using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Interview
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Tags",
               url: "Posts/Tags/{tag}",
               defaults: new { controller = "Posts", action = "Tags", tag = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Users",
                url: "Users/{action}/{username}",
                defaults: new { controller = "Users", action = "AllPosts", username = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
