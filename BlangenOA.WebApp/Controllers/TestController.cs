using BlangenOA.BLL;
using BlangenOA.Common;
using BlangenOA.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            int a = 1, b = 0;
            int c = a / b;
            return Content(c.ToString());
        }
        /// <summary>
        /// 测试memcached
        /// </summary>
        /// <returns></returns>
        public ActionResult Memcached()
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            //if (MemcacheHelper.Delete("test"))
            //{
            //    str1 = "原来的给我删了！";
            //}
            str2 = "空滴";
            if (MemcacheHelper.Get("test") != null)
            {
                str2 = MemcacheHelper.Get("test").ToString();
            }
            else
            {
                MemcacheHelper.Set("test", "我有值啦~", DateTime.Now.AddMinutes(20));
            }
            return Content(str1 + str2);
        }
        /// <summary>
        /// 测试IndexManager
        /// </summary>
        /// <returns></returns>
        public ActionResult TestCreate()
        {
            Model.Books model = new Model.Books();
            model.AurhorDescription = "jlkfdjf";
            model.Author = "asfasd";
            model.CategoryId = 1;
            model.Clicks = 1;
            model.ContentDescription = "卢本伟牛逼,披着国旗在国际舞台上叱咤风云！";
            model.EditorComment = "adfsadfsadf";
            model.ISBN = "111111111111111111";
            model.PublishDate = DateTime.Now;
            model.PublisherId = 72;
            model.Title = "卢本伟的一生";
            model.TOC = "aaaaaaaaaaaaaaaa";
            model.UnitPrice = 22.3m;
            model.WordsCount = 1234;
            //将数据先存储到数据库中。获取刚插入的数据的主键ID值。
            IBLL.IBooksBll booksBll = new BooksBll();
            model = booksBll.AddEntity(model);
            IndexManager.CreateInstance().AddToLucene(model.Id, model.Title, model.ContentDescription);//向队列中添加
            return Content("ok");
        }
    }
}