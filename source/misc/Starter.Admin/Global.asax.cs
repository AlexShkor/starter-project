using DQF.Platform.StructureMap;
using StructureMap;
using System.Web.Mvc;
using System.Web.Routing;

namespace DQF.Admin
{
    public class MooreCoalAdminApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = HttpApplicationStructureMapContext.Current;
            new Bootstaper().Configure(container);

            ConfigureMvc(container);
        }

        private void ConfigureMvc(IContainer container)
        {
            AreaRegistration.RegisterAllAreas();

            // Registering filters
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            // Registing routes
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            // Configure resolver for MVC controllers, filters etc.
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
        }
    }
}