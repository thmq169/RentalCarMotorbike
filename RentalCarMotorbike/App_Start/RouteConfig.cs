using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RentalCarMotorbike
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Authent", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "RentalCarMotorbike.Controllers" }  
            );

            /*routes.MapRoute(
                name: "areas",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Authent", action = "Login", id = UrlParameter.Optional },
            );*/
        }
    }
}
