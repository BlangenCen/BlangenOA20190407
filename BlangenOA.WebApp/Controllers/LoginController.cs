using BlangenOA.Common;
using BlangenOA.IBLL;
using BlangenOA.Model;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp.Controllers
{
    public class LoginController : Controller
    {
        public IUserInfoBll UserInfoBll { get; set; }
        // GET: Login
        public ActionResult Index()
        {
            //检查Session是否存在
            //if (Session["UserInfo"] == null)
            if (Request["sessionID"] == null)
            {
                //检查Cookie储存的用户信息
                if (Request.Cookies["UserName"] != null)
                {
                    string userName = Request.Cookies["UserName"].Value;
                    //IOC
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IUserInfoBll userInfoBll = (IUserInfoBll)ctx.GetObject("UserInfoBll");
                    UserInfo userInfo = userInfoBll.LoadEntities(u => u.UName == userName).FirstOrDefault();
                    if (userInfo != null)
                    {
                        if (WebCommon.ValidateUserInfoCookie(userInfo))
                        {
                            //信息正确 跳转主界面
                            return Redirect(Url.Action("Index", "Home"));
                        }
                    }
                }
            }
            else
            {
                string sessionID = Request.Cookies["sessionID"].Value;
                //获取Memcache中的数据
                object obj = MemcacheHelper.Get(sessionID);
                if (obj != null)
                {
                    //反序列化存储在Memcache的用户
                    UserInfo userInfo = SerializeHelper.DeSerializeToT<UserInfo>(obj.ToString());
                    //模拟滑动过期时间。
                    MemcacheHelper.Set(sessionID, obj, DateTime.Now.AddMinutes(20));
                    return Redirect(Url.Action("Index", "Home"));
                }
            }
            return View();
        }
        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowValidateCode()
        {
            ValidateCode vc = new ValidateCode();
            //产生验证码
            string vcStr = vc.CreateValidateCode(5);
            //存储到Session中
            Session["ValidateCode"] = vcStr;
            //将验证码画到画布上
            byte[] b = vc.CreateValidateGraphic(vcStr);
            return File(b, "image/jpeg");
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin()
        {
            string validateCode = Session["ValidateCode"] == null ? string.Empty : Session["ValidateCode"].ToString();
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("no:验证码错误！");
            }
            //记得要设null
            Session["ValidateCode"] = null;
            string txtCode = Request["vCode"];
            if (!(txtCode.Equals(validateCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                return Content("no:验证码错误！");
            }
            //LoginCode LoginPwd vCode
            string UserName = Request["LoginCode"];
            string Password = Request["LoginPwd"];
            var userInfo = UserInfoBll.LoadEntities(u => u.UName == UserName && u.UPwd == Password).FirstOrDefault();
            if (userInfo == null)
            {
                return Content("no:用户名/密码错误！");
            }
            else
            {
                //Session["UserInfo"] = userInfo;
                WebCommon.SaveToMemcache(userInfo);
                //判断记住密码是否被勾选
                if (!string.IsNullOrEmpty(Request["autoLogin"]))
                {
                    HttpCookie c1 = new HttpCookie("UserName", UserName)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    };
                    //应该MD5加密两次的
                    HttpCookie c2 = new HttpCookie("Password", Password)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    };
                    Response.Cookies.Add(c1);
                    Response.Cookies.Add(c2);
                }
                return Content("ok:登陆成功");
            }
        }
    }
}