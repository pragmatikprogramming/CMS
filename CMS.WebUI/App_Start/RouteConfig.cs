using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new{ id = "^[0-9]+$"}
            );

            routes.MapRoute(
                name: "Admin",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteFAQQuestion",
                url: "{controller}/{action}/{parentId}/{id}",
                defaults: new { controller = "FAQ", action = "Index", parentId = 0, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Three Values",
                url: "{controller}/{action}/{parentId}/{id}/{value}",
                defaults: new { controller = "Form", action = "Index", parentId = 0, id = 0, value = UrlParameter.Optional }
            );
        }
    }
}