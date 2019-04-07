using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using ServiceStack.Redis;

namespace BlangenOA.WebApp.Models
{
    public class MyExceptionAttribute : HandleErrorAttribute
    {
        //公有静态，保证出错后使用的是同一个队列
        //public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
        public static IRedisClientsManager clientManager = new PooledRedisClientManager(new string[] { "172.18.38.99:6379" });
        public static IRedisClient redisClient = clientManager.GetClient();
        /// <summary>
        /// 可以捕获异常数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            #region 测试用
            //using (FileStream f = new FileStream(@"C:\Users\Administrator\Desktop\1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //{
            //    byte[] b = Encoding.UTF8.GetBytes("你看看我是谁");
            //    f.Write(b, 0, b.Count());
            //} 
            #endregion
            //ex入队
            //ExceptionQueue.Enqueue(ex);
            redisClient.EnqueueItemOnList("ExceptionQueue", ex.ToString());
            //跳转到错误页
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}