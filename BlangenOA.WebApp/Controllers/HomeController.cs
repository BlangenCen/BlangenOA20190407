using BlangenOA.IBLL;
using BlangenOA.Model;
using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp.Controllers
{
    public class HomeController : BaseController//Controller
    {
        IUserInfoBll UserInfoBll { get; set; }
        public ActionResult Index()
        {
            //用于实现(欢迎***登录)这个功能
            ViewBag.UserName = LoginUserInfo.UName;
            return View();
        }
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Homepage()
        {
            return View();
        }
        /// <summary>
        /// 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
        /// 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
        /// 3：将这两个集合合并成一个集合。
        /// 4：把禁止的权限从总的集合中清除。
        /// 5：将总的集合中的重复权限清除。
        /// 6：把过滤好的菜单权限生成JSON返回。
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenus()
        {
            // 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
            //获取登录用户的信息
            var userInfo = UserInfoBll.LoadEntities(u => u.ID == LoginUserInfo.ID).FirstOrDefault();
            //获取登录用户角色
            var userRoleInfo = userInfo.RoleInfo;
            //获取用户角色对应的菜单权限
            short actionEnumType = (short)ActionEnumType.Menu;//菜单权限
            var temp1 = (from r in userRoleInfo
                         from a in r.ActionInfo
                         where a.ActionTypeEnum == actionEnumType
                         select a).ToList();

            // 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
            var temp2 = (from a in userInfo.R_UserInfo_ActionInfo
                         where a.ActionInfo.ActionTypeEnum == actionEnumType
                         select a.ActionInfo).ToList();

            // 3：将这两个集合合并成一个集合。
            temp1.AddRange(temp2);

            // 4：把禁止的权限从总的集合中清除。（两条路线都有可能拿到被禁止的权限）
            var forbidActionsIDList = (from a in userInfo.R_UserInfo_ActionInfo
                                  where a.IsPass == false
                                  select a.ActionInfoID).ToList();
            var temp3 = temp1.Where(a => !forbidActionsIDList.Contains(a.ID));

            // 5：将总的集合中的重复权限清除。
            var temp4 = temp3.Distinct(new ActionInfoEqualityComparer());

            // 6：把过滤好的菜单权限生成JSON返回。
            var userMenuActionList = from a in temp4
                                     select new
                                     {
                                         icon = a.MenuIcon,
                                         title = a.ActionInfoName,
                                         url = a.Url
                                     };

            return Json(userMenuActionList, JsonRequestBehavior.AllowGet);
        }




        //用来测试Newtonsoft.Json有无序列化主键表
        //public ActionResult Test()
        //{
        //    return Content(LoginUserInfo.RoleInfo.Take(1).FirstOrDefault().RoleName);
        //}
    }
}