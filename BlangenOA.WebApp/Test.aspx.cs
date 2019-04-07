using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlangenOA.WebApp
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Request.Url.AbsolutePath);
            Response.Write("<hr />");
            Response.Write(Request.Url);
            Response.Write("<hr />");
            Response.Write(Request.RawUrl);
            Response.Write("<hr />");
            Response.Write(Request.UrlReferrer);
        }
    }
}