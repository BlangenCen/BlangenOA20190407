using BlangenOA.WebApp.Models;
using System.Web;
using System.Web.Mvc;

namespace BlangenOA.WebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new MyExceptionAttribute());
            //ActionFilterAttribute
            //AuthorizeAttribute
        }
    }
}
