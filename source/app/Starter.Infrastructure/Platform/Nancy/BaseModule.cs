using DQF.Common.Account;
using Nancy;
using System.Linq;

namespace DQF.Platform.Nancy
{
    public abstract class BaseModule : NancyModule
    {
        private string _baseUrl;
        protected string BaseUrl
        {
            get
            {
                if (_baseUrl == null)
                {
                    var url = Request.Url;
                    var addPort = !url.HostName.Contains(".") && url.Port.HasValue;
                    _baseUrl = string.Format("{0}://{1}{2}", url.Scheme, url.HostName, addPort ? ":" + url.Port : "");
                }
                return _baseUrl;
            }
        }

        protected UserIdentity CurrentUser
        {
            get { return Context.CurrentUser as UserIdentity; }
        }


        protected BaseModule()
        {
            FillViewBag();
        }

        protected BaseModule(string modulePath)
            : base(modulePath)
        {
            FillViewBag();
        }

        private void FillViewBag()
        {
            After.AddItemToEndOfPipeline(context =>
            {
                ViewBag.BaseUrl = BaseUrl;
                if (context.CurrentUser != null)
                {
                    context.ViewBag.UserName = context.CurrentUser.UserName;
                    var claims = context.CurrentUser.Claims.ToList();
                    context.ViewBag.IsAdmin = claims.Contains(Claims.Admin);
                }
            });
        }

        protected Response NotFound()
        {
            return new Response { StatusCode = HttpStatusCode.NotFound };
        }
    }
}