using System.Web.Mvc;
using DQF.Common.Account;

namespace DQF.Platform.Mvc
{
    public class AuthAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = httpContext.Session[BaseController.UserIdentitySessionKey] as UserIdentity;
            return user != null;
        }
    }
}