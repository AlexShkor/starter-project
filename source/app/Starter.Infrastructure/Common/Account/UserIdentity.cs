using System.Collections.Generic;
using DQF.Domain.Aggregates.User;
using DQF.Platform.Extensions;
using DQF.Views;
using Nancy.Security;

namespace DQF.Common.Account
{
    public class UserIdentity
    {

        public UserRoleEnum Role { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public UserIdentity(UserView view)
        {
            UserId = view.Id;
            UserName = view.UserName;
            Email = view.Email;
            Role = view.Role;
        }

        public bool IsGlobalAdmin
        {
            get { return Role == UserRoleEnum.GlobalAdmin; }
        }
    }
}