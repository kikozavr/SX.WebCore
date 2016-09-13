using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace SX.WebCore.MvcApplication
{
    class SxRouteConfig
    {
        public static void RegisterRoutes(
            RouteCollection routes, string[] dafaultNamespaces,
            Action<RouteCollection> preRouteAction,
            Action<RouteCollection> postRouteAction
            )
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //pre
            if(preRouteAction!=null)
                preRouteAction(routes);

            //core routes
            routes.MapRoute(
                name: "robots",
                url: "robots.txt",
                defaults: new { controller = "Seo", action = "Robotstxt", area = "" },
                namespaces: dafaultNamespaces
            );

            routes.MapRoute(
                name: "sitemap",
                url: "sitemap.xml",
                defaults: new { controller = "Seo", action = "Sitemap", area = "" },
                namespaces: dafaultNamespaces
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "Articles", action = "Details", area = "" },
                namespaces: dafaultNamespaces
            );

            routes.MapRoute(
                name: null,
                url: "Articles",
                defaults: new { controller = "Articles", action = "List", page = 1, area = "" },
                namespaces: dafaultNamespaces
            );

            //post
            if(postRouteAction!=null)
                postRouteAction(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "" },
                namespaces: dafaultNamespaces
            );
        }
    }
}
