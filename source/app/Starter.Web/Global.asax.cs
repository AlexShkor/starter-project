using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DQF.Common.Account;
using DQF.ViewServices;
using StructureMap;

namespace DQF.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = false;
            _container = new Container();
            DependencyConfig.RegisterDependencies(_container);
        }

        //protected void Application_BeginRequest()
        //{
        //    var principal = HttpContext.Current.User;
        //   if (principal != null)
        //   {
        //       var user = _container.GetInstance<UsersViewService>().GetByEmail(principal.Identity.Name);
        //       HttpContext.Current.Session["UserIdentity"] = new UserIdentity(user);
        //   }
        //}
        //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //}
    }
}