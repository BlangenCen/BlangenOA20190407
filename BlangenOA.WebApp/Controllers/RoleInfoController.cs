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
    public class RoleInfoController : BaseController//Controller
    {
        IRoleInfoBll RoleInfoBll { get; set; }
        IActionInfoBll ActionInfoBll { get; set; }
        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 展示角色信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleInfoList()
        {
            //请求报文里有两个参数:page和rows
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 5 : int.Parse(Request["rows"]);
            var temp = RoleInfoBll.LoadPageEntities(pageIndex, pageSize, out int pageCount, true, r => r.DelFlag == (short)DeleteEnumType.Normal, r => r.ID);
            var roleInfo = from r in temp
                           select new
                           {
                               r.ID,
                               r.RoleName,
                               r.Remark,
                               r.SubTime,
                               r.Sort
                           };
            return Json(new { total = pageCount, rows = roleInfo });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteRoleInfo()
        {
            List<int> idList = new List<int>();
            foreach (string id in Request["strID"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                idList.Add(Convert.ToInt32(id));
            }
            if (RoleInfoBll.DeleteEntities(idList))
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 展示添加表单
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowAdd()
        {
            return View();
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public ActionResult AddRoleInfo(RoleInfo r)
        {
            r.DelFlag = (short)DeleteEnumType.Normal;
            r.SubTime = DateTime.Now;
            r.ModifiedOn = DateTime.Now;
            if (RoleInfoBll.AddEntity(r) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        /// <summary>
        /// 返回指定ID对象
        /// </summary>
        /// <returns></returns>
        public ActionResult FindRoleInfo()
        {
            int id = Convert.ToInt32(Request["ID"]);
            //这个FindEntity到底怎么用的？
            //var userInfo = Bll.FindEntity(new UserInfo() { ID = id });
            var roleInfo = RoleInfoBll.LoadEntities(r => r.ID == id).FirstOrDefault();
            return Json(roleInfo);
        }
        /// <summary>
        /// 展示编辑表单
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowEdit()
        {
            int id = Convert.ToInt32(Request["id"]);
            var roleInfo = RoleInfoBll.LoadEntities(r => r.ID == id).FirstOrDefault();
            ViewBag.roleInfo = roleInfo;
            return View();
        }
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult EditRoleInfo(RoleInfo entity)
        {
            entity.ModifiedOn = DateTime.Now;
            //entity.SubTime = DateTime.Now;
            if (RoleInfoBll.EditEntity(entity) != null)
            {
                return Content("ok");
            }
            else
            {
                return Content("error");
            }
        }
        /// <summary>
        /// 展示角色权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRoleAction()
        {
            //当前角色ID
            int roleID = Convert.ToInt32(Request["roleID"]);
            //当前角色
            var roleInfo = RoleInfoBll.LoadEntities(u => u.ID == roleID).FirstOrDefault();
            //当前角色已经拥有的权限
            var roleActionIDList = (from a in roleInfo.ActionInfo
                                    select a.ID).ToList();
            //获取所有权限
            var actionList = ActionInfoBll.LoadEntities(a => a.DelFlag == (short)DeleteEnumType.Normal).ToList();
            //放在ViewBag
            ViewBag.roleInfo = roleInfo;
            ViewBag.roleActionIDList = roleActionIDList;
            ViewBag.actionList = actionList;
            return View();
        }
        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRoleAction()
        {
            //当前角色ID
            int roleID = Convert.ToInt32(Request["roleID"]);
            //存储着actionID_id
            string[] temp = Request.Form.AllKeys;
            //用来存储被勾选的actionID
            List<int> roleActionIDList = new List<int>();
            foreach (string item in temp)
            {
                if (item.StartsWith("actionID_"))
                {
                    string id = item.Replace("actionID_", "");
                    roleActionIDList.Add(Convert.ToInt32(id));
                }
            }
            return RoleInfoBll.SetRoleAction(roleID, roleActionIDList) ? Content("ok") : Content("error");
        }
    }
}