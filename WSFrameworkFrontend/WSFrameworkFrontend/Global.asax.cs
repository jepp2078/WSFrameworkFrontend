using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WSFrameworkFrontend
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {
            var cookie = Request.Cookies["AccessToken"];

            if (cookie != null && cookie.Value != null)
                Session["AccessToken"] = cookie.Value;
        }
    }
}
