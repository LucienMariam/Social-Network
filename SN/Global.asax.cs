using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using WebMatrix.WebData;
using SN.Models;
namespace SN
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string MyHost = "http://localhost:23746/";
        
        protected void Application_Start()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteTable.Routes.MapRoute("ControllerActionUsername", "{controller}/{action}/{Username}");
            RouteTable.Routes.MapRoute("Confirmation", "{controller}/{action}/{username}/{confirmation}", new { controller = "Action", action = "Confirmation" });
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            
        }
    }
}