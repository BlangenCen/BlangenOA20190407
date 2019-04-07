using BlangenOA.IBLL;
using BlangenOA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp.Controllers
{
    public class ActionInfoController : BaseController//Controller
    {
        IActionInfoBll ActionInfoBll { get; set; }
        // GET: ActionInfo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 展示数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetActionInfoList()
        {
            //请求报文里有两个参数:page和rows
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 5 : int.Parse(Request["rows"]);
            var temp = ActionInfoBll.LoadPageEntities<int>(pageIndex, pageSize, out int pageCount, true, u => u.DelFlag == (short)DeleteEnumType.Normal, a => a.ID);
            var userInfoList = from a in temp
                               select new
                               {
                                   a.ID,
                                   a.SubTime,
                                   a.Remark,
                                   a.Url,
                                   a.HttpMethod,
                                   a.ActionInfoName,
                                   a.ActionTypeEnum
    };
            return Json(new { total = pageCount, rows = userInfoList });
        }
    }
}