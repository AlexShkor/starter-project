using System.IO;
using System.Linq;
using Nancy;
using Nancy.ViewEngines;
using Nancy.ViewEngines.Razor;

namespace DQF.Platform.Nancy
{
    public class NancyRazorStringRenderer
    {
        private readonly DefaultViewLocationCache _viewLocationCache;
        private readonly RazorViewEngine _engine;
        private readonly DefaultRenderContextFactory _renderContextFactory;

        public NancyRazorStringRenderer(DefaultViewLocationCache viewLocationCache, RazorViewEngine engine, DefaultRenderContextFactory renderContextFactory)
        {
            _viewLocationCache = viewLocationCache;
            _engine = engine;
            _renderContextFactory = renderContextFactory;
        }

        public string Render(string viewName, object model, NancyContext context)
        {
            var renderContext = _renderContextFactory.GetRenderContext(new ViewLocationContext
            {
                Context = context,
                ModuleName = context.NegotiationContext.ModuleName,
                ModulePath = context.NegotiationContext.ModulePath,
            });

            Response result = _engine.RenderView(_viewLocationCache.First(x => x.Name == viewName), model, renderContext);

            var mem = new MemoryStream();
            result.Contents.Invoke(mem);
            mem.Position = 0;
            var reader = new StreamReader(mem);

            return reader.ReadToEnd();
        }
    }
}