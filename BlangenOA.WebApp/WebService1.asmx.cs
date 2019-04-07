using BlangenOA.IBLL;
using BlangenOA.Model.EnumType;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BlangenOA.WebApp
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public int Add(int a, int b)
        {
            return a + b;
        }
        [WebMethod]
        public string GetUserInfo()
        {
            IApplicationContext ctx = ContextRegistry.GetContext();
            IUserInfoBll userInfoBll = (IUserInfoBll)ctx.GetObject("UserInfoBll");
            var userInfo = userInfoBll.LoadEntities(u => u.DelFlag == (short)DeleteEnumType.Normal).ToList();
            return Common.SerializeHelper.SerializeToString(userInfo);
        }
    }
}
