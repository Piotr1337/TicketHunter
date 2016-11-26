using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TicketHunter
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "{controller}/{action}/{categoryName}/{categoryId}/{subcategoryName}/{subcategoryId}", new
            {
                controller = "Event",
                action = "List",
                categoryName = UrlParameter.Optional,
                categoryId = UrlParameter.Optional,
                subcategoryName = UrlParameter.Optional,
                subcategoryId = UrlParameter.Optional
            });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Event", action = "List", id = UrlParameter.Optional }
                );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
