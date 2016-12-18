using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TicketHunter.Controllers.AdminController;
using TicketHunter.Domain.Abstract;
using TicketHunter.Domain.Scheduler;
using TicketHunter.Infrastructure;
using TicketHunter.Mappings;

namespace TicketHunter
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();
            var controller = DependencyResolver.Current.GetService<AdminController>();
            controller.Scheduler();
        }
    }
}
