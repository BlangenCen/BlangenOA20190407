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
    public class UserInfoController : BaseController//Controller 
    {
        //readonly UserInfoBll Bll = new UserInfoBll();
        IUserInfoBll UserInfoBll { get; set; }
        IRoleInfoBll RoleInfoBll { get; set; }
        IActionInfoBll ActionInfoBll { get; set; }
        IR_UserInfo_ActionInfoBll R_UserInfo_ActionInfoBll { get; set; }
        // GET: UserInfo
        public ActionResult Index()
        {
            //var temp = Bll.LoadEntities(u => u.ID > 0);
            //return Content(temp.ToList()[0].UName);
            return View();
        }

        /// <summary>
        /// 展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfoList()
        {
            //请求报文里有两个参数:page和rows
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 5 : int.Parse(Request["rows"]);
            //var temp = Bll.LoadPageEntities<int>(pageIndex, pageSize, out int pageCount, true, u => u.DelFlag == (short)DeleteEnumType.Normal, u => u.ID);
            UserInfoSearch userInfoSearch = new UserInfoSearch()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                UserName = Request["name"],
                UserRemark = Request["remark"]
            };
            //执行完下面这个方法后userInfoSearch.PageCount才有值
            var temp = UserInfoBll.LoadSearchEntities(userInfoSearch, DeleteEnumType.Normal);
            var userInfoList = from u in temp
                               select new
                               {
                                   //下面可以注释的原因：自己看吧
                                   /*ID = */
                                   u.ID,
                                   /*UName = */
                                   u.UName,
                                   /*UPwd = */
                                   u.UPwd,
                                   /*Remark = */
                                   u.Remark,
                                   SubTime = u.SubTime
                               };
            return Json(new { total = userInfoSearch.PageCount, rows = userInfoList });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteUserInfo()
        {
            List<int> idList = new List<int>();
            foreach (string id in Request["strID"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                idList.Add(Convert.ToInt32(id));
            }
            if (UserInfoBll.DeleteEntities(idList))
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUserInfo(UserInfo entity)
        {
            entity.DelFlag = (short)DeleteEnumType.Normal;
            entity.SubTime = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            if (UserInfoBll.AddEntity(entity) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 返回指定ID对象
        /// </summary>
        /// <returns></returns>
        public ActionResult FindUserInfo()
        {
            int id = Convert.ToInt32(Request["ID"]);
            //这个FindEntity到底怎么用的？
            //var userInfo = Bll.FindEntity(new UserInfo() { ID = id });
            var temp = UserInfoBll.LoadEntities(u => u.ID == id).FirstOrDefault();
            //去除导航属性
            var userInfo = new
            {
                temp.ID,
                temp.DelFlag,
                temp.SubTime,
                temp.UName,
                temp.UPwd,
                temp.Remark,
                temp.Sort
            };
            return Json(userInfo);
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult EditUserInfo(UserInfo entity)
        {
            entity.ModifiedOn = DateTime.Now;
            //entity.SubTime = DateTime.Now;
            if (UserInfoBll.EditEntity(entity) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 展示用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowUserRole()
        {
            //当前用户ID
            int id = Convert.ToInt32(Request["id"]);
            //当前用户
            var userInfo = UserInfoBll.LoadEntities(u => u.ID == id).FirstOrDefault();
            //当前用户已经拥有的角色
            var userRoleInfoIDList = (from r in userInfo.RoleInfo
                                      select r.ID).ToList();
            //获取所有角色
            var roleInfoList = RoleInfoBll.LoadEntities(r => r.DelFlag == (short)DeleteEnumType.Normal).ToList();
            //放在ViewBag
            ViewBag.userInfo = userInfo;
            ViewBag.userRoleInfoIDList = userRoleInfoIDList;
            ViewBag.roleInfoList = roleInfoList;
            return View();
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserRole()
        {
            //当前用户ID
            int userID = Convert.ToInt32(Request["userID"]);
            //存储着roleID_id
            string[] temp = Request.Form.AllKeys;
            //用来存储被勾选的roleID
            List<int> userRoleIDList = new List<int>();
            foreach (string item in temp)
            {
                if (item.StartsWith("roleID_"))
                {
                    string id = item.Replace("roleID_", "");
                    userRoleIDList.Add(Convert.ToInt32(id));
                }
            }
            return UserInfoBll.SetUserRole(userID, userRoleIDList) ? Content("ok") : Content("error");
        }
        /// <summary>
        /// 展示用户权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowUserAction()
        {
            //当前用户ID
            int userID = Convert.ToInt32(Request["userID"]);
            //当前用户
            var userInfo = UserInfoBll.LoadEntities(u => u.ID == userID).FirstOrDefault();
            //当前用户已经拥有的权限（中间表）
            var userActionList = (from a in userInfo.R_UserInfo_ActionInfo
                                  select a).ToList();
            //获取所有权限
            var actionList = ActionInfoBll.LoadEntities(a => a.DelFlag == (short)DeleteEnumType.Normal).ToList();
            //放在ViewBag
            ViewBag.userInfo = userInfo;
            ViewBag.userActionList = userActionList;
            ViewBag.actionList = actionList;
            return View();
        }
        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserAction()
        {
            //用户ID
            int userID = Convert.ToInt32(Request["userID"]);
            //权限ID
            int actionID = Convert.ToInt32(Request["actionID"]);
            //允许/禁止
            bool isPass = Request["isPass"] == "true" ? true : false;
            if (UserInfoBll.SetUserAction(userID, actionID, isPass))
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 清除权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearUserAction()
        {
            //用户ID
            int userID = Convert.ToInt32(Request["userID"]);
            //权限ID
            int actionID = Convert.ToInt32(Request["actionID"]);
            //用户权限表中的一条记录
            var userAction = R_UserInfo_ActionInfoBll.LoadEntities(uA => uA.UserInfoID == userID && uA.ActionInfoID == actionID).FirstOrDefault();
            //用户存在此权限
            if (userAction != null)
            {
                if (R_UserInfo_ActionInfoBll.DeleteEntity(userAction))
                {
                    return Content("ok:清除成功！");
                }
                return Content("error:清除失败！");
            }
            else
            {
                return Content("error:已经清除了,别乱按！");
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["sessionID"].Expires = DateTime.Now.AddDays(-1);
            return Redirect("/Login/Index");
        }
    }
}