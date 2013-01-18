using System.Web.Mvc;
using DQF.Platform.StructureMap;
using StructureMap;

namespace DQF.Web
{
    public class DependencyConfig
    {
        public static void RegisterDependencies(IContainer container)
        {
            new Bootstaper().Configure(container);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
        }
    }
}