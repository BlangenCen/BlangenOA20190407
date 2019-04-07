using BlangenOA.BLL;
using BlangenOA.IBLL;
using BlangenOA.Model;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.PanGu;
using PanGu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlangenOA.Common
{
    public static class WebCommon
    {
        //每次使用的context都不一样,所以还是在方法中赋值吧
        //（也可以在BaseController以参数形式传给方法）
        static HttpContext Context { get; set; }
        static IUserInfoBll userInfoBll = new UserInfoBll();
        static IActionInfoBll actionInfoBll = new ActionInfoBll();
        /// <summary>
        /// 校验Cookie
        /// </summary>
        /// <param name="u">用户</param>
        /// <returns>Cookie信息是否正确</returns>
        public static bool ValidateUserInfoCookie(UserInfo u)
        {
            Context = HttpContext.Current;
            bool isSuccess = false;
            if (u != null)
            {
                if (Context.Request.Cookies["Password"] != null)
                {
                    string password = Context.Request.Cookies["Password"].Value;
                    if (password == u.UPwd)
                    {
                        isSuccess = true;
                        return isSuccess;
                    }
                }
            }
            Context.Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Context.Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
            return isSuccess;
        }
        /// <summary>
        /// 用Memcache存储数据，键为"sessionID"
        /// </summary>
        /// <param name="userInfo">用户对象</param>
        public static void SaveToMemcache(UserInfo userInfo)
        {
            Context = HttpContext.Current;
            //保证Memcache的key唯一
            string sessionID = Guid.NewGuid().ToString();
            //向Memcache中添加登录用户数据.
            MemcacheHelper.Set(sessionID, SerializeHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(20));
            //将自创的SessionId以Cookie的形式返回给浏览器。
            Context.Response.Cookies["sessionID"].Value = sessionID;
        }
        /// <summary>
        /// 检验用户是否有权限访问，并返回访问地址
        /// </summary>
        /// <returns></returns>
        public static string CheckLoginUserAction(UserInfo loginUserInfo)
        {
            //return true;
            //每次的context都不一样！所以不能用这个类中的context
            Context = HttpContext.Current;
            //格式-->/xxx/xx（没有域名的,即没有http://localhost:61766/）
            string url = Context.Request.Url.AbsolutePath.ToLower();//数据库中存的是小写
            if (url == "/")
            {
                //说明访问的是首页
                return url;
            }
            if (url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1] == null)
            {
                //说明访问的是控制器的index
                url += "/index";
            }
            string httpMethod = Context.Request.HttpMethod.ToUpper();//数据库存的是大写
            var actionInfo = actionInfoBll.LoadEntities(a => a.Url == url && a.HttpMethod == httpMethod).FirstOrDefault();
            //系统中不存在此权限
            if (actionInfo == null)
            {
                return null;
            }
            var userInfo = userInfoBll.LoadEntities(u => u.ID == loginUserInfo.ID).FirstOrDefault();
            //系统中不存在此用户
            if (userInfo == null)
            {
                return null;
            }
            //1.用户->权限
            var isExist = (from a in userInfo.R_UserInfo_ActionInfo
                           where a.ActionInfoID == actionInfo.ID
                           select a).FirstOrDefault();
            //用户存在此权限
            if (isExist != null)
            {
                //此权限被禁止
                if (!isExist.IsPass)
                {
                    return null;
                }
                else
                {
                    return url;
                }
            }
            //2.用户->角色->权限（能走到这,说明此权限没被禁止！）
            var userRole = userInfo.RoleInfo;
            var count = (from r in userRole
                         from a in r.ActionInfo
                         where a.ID == actionInfo.ID
                         select a).Count();
            if (count > 0)
            {
                return url;
            }
            return null;
        }
        /// <summary>
        /// 盘古分词
        /// </summary>
        /// <param name="msg">需要进行拆分的字符串</param>
        /// <returns>拆分结果</returns>
        public static List<string> PanguSplitWords(string msg)
        {
            List<string> list = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(msg));
            Lucene.Net.Analysis.Token token = null;
            while ((token = tokenStream.Next()) != null)
            {
                list.Add(token.TermText());
            }
            return list;
        }
        /// <summary>
        /// 创建HTMLFormatter,参数为高亮单词的前后缀
        /// </summary>
        /// <param name="msg">原始搜索内容</param>
        /// <param name="Content">字段里的值</param>
        /// <returns></returns>
        public static string CreateHightLight(string msg, string Content)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
             new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"red\">", "</font>");
            //创建Highlighter ，输入HTMLFormatter 和盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 150;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(msg, Content);

        }
    }
}
