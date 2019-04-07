using BlangenOA.IBLL;
using BlangenOA.Model;
using Spring.Context;
using Spring.Context.Support;
using System.Web.Mvc;
using System.Linq;
using System;
using BlangenOA.Common;

namespace BlangenOA.WebApp.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo LoginUserInfo { get; set; }
        /// <summary>
        /// 执行控制器中的方法之前先执行该方法。
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
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
                        if (!WebCommon.ValidateUserInfoCookie(userInfo))
                        {
                            //信息不正确 跳转登录页面（可以提醒用户Cookie失效）
                            filterContext.Result = Redirect(Url.Action("Index", "Login"));
                        }
                        else
                        {
                            //Session["UserInfo"] = userInfo;
                            WebCommon.SaveToMemcache(userInfo);
                            LoginUserInfo = userInfo;
                            if (LoginUserInfo.UName == "BlangenCen")
                            {
                                return;
                            }
                            //SessionID过期，且Cookie存有正确的用户信息，访问主页
                            string requestUrl = WebCommon.CheckLoginUserAction(userInfo);
                            if (string.IsNullOrEmpty(requestUrl))
                            {
                                //用户没有权限访问
                                filterContext.Result = Redirect("/NoAction.html");
                            }
                            else
                            {
                                filterContext.Result = Redirect(requestUrl);
                            }
                        }
                    }
                    else
                    {
                        //并提醒用户Cookie失效
                        filterContext.Result = Redirect("/Login/Index");
                    }
                }
                else
                {
                    //filterContext.Result = Redirect("/Login/Index");
                    filterContext.Result = Redirect(Url.Action("Index", "Login"));
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
                    LoginUserInfo = userInfo;
                    if (LoginUserInfo.UName == "BlangenCen")
                    {
                        return;
                    }
                    string requestUrl = WebCommon.CheckLoginUserAction(userInfo);
                    if (string.IsNullOrEmpty(requestUrl))
                    {
                        //用户没有权限访问
                        filterContext.Result = Redirect("/NoAction.html");
                    }
                    else
                    {
                        //filterContext.Result = Redirect(requestUrl);
                        return;//这里直接return，就会开始执行方法了~  不要滥用filterContext.Result！！！
                    }
                }
                else
                {
                    //没有用户信息，跳转登录页面
                    filterContext.Result = Redirect(Url.Action("Index", "Login"));
                }
            }
        }
    }
}