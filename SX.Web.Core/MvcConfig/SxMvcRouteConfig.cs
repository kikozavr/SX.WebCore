using System.Web.Mvc;
using System.Web.Routing;

namespace SX.Web.Core.MvcConfig
{
    public abstract class SxMvcRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, area="" }
            );
        }
    }
}
