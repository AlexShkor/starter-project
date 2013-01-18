using System.Collections.Generic;
using System.Web.Mvc;
using DQF.Common.Account;
using DQF.Domain.Aggregates.User;

namespace DQF.Platform.Mvc
{
    public class RolesAttribute: AuthorizeAttribute
    {
        private readonly List<UserRoleEnum> _roles;

        public RolesAttribute(params UserRoleEnum[] roles)
        {
            _roles = new List<UserRoleEnum>(roles);
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var user = httpContext.Session[BaseController.UserIdentitySessionKey] as UserIdentity;
            if ( user != null)
            {
                if (_roles.Contains(user.Role))
                {
                    return true;
                }
            }
            return false;
        }
    }
}