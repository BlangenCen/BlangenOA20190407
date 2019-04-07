using BlangenOA.WebApp.Models;
using log4net;
using Spring.Web.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BlangenOA.WebApp
{
    public class MvcApplication : SpringMvcApplication //System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //读取了配置文件中关于Log4Net配置信息.
            log4net.Config.XmlConfigurator.Configure();
            //开始线程扫描LuceneNet对应的数据队列。
            IndexManager.CreateInstance().StartThread();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //开启一个线程，扫描异常信息队列
            ThreadPool.QueueUserWorkItem(HandleExceptionQueue, Server.MapPath("/Log"));
        }
        /// <summary>
        /// 处理Ex队列
        /// </summary>
        /// <param name="logDirectory">日志文件目录</param>
        private void HandleExceptionQueue(object logDirectory)
        {
            while (true)
            {
                //if (MyExceptionAttribute.ExceptionQueue.Count() > 0)
                if (MyExceptionAttribute.redisClient.GetListCount("ExceptionQueue") > 0)
                {
                    //Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();
                    string exMsg = MyExceptionAttribute.redisClient.DequeueItemFromList("ExceptionQueue");
                    //if (ex != null)
                    if (!string.IsNullOrEmpty(exMsg))
                    {
                        #region 手动日志记录
                        ////将异常信息写到日志文件中。
                        //string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                        //string fileFullPath = Path.Combine(logDirectory.ToString(), fileName);
                        ////ex.ToString()全部异常信息 ex.Message异常信息的简短描述
                        //File.AppendAllText(fileFullPath, ex.ToString() + "\r\n\r\n", Encoding.UTF8); 
                        #endregion
                        //log4net
                        ILog logger = LogManager.GetLogger("errorMessage");
                        //logger.Error(ex.Message);
                        //logger.Error(ex.ToString());
                        logger.Error(exMsg.ToString());
                    }
                }
                else
                {
                    //很关键,队列为空时等一段时间后，线程再继续执行，降低内存占用
                    Thread.Sleep(3000);
                }
            }
        }

    }
}
