using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace webleitour
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BookPage",
                url: "Book/bookpage/{ISBN}",
                defaults: new { controller = "Book", action = "BookPage", ISBN = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Registrar",
                url: "User/Registrar",
                defaults: new { controller = "User", action = "Registrar" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
