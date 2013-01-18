using System;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Helpers;
using DQF.Platform.Domain;
using DQF.ViewServices;
using FormsAuthentication = System.Web.Security.FormsAuthentication;

namespace DQF.Common.Account
{
    public class Claims
    {
        public const string Admin = "Admin";
    }

    public class AuthenticationService
    {
        private readonly UsersViewService _users;
        private readonly CryptographicHelper _crypto;
        private readonly ICommandBus _bus;

        public AuthenticationService(UsersViewService users, CryptographicHelper crypto, ICommandBus bus)
        {
            _users = users;
            _crypto = crypto;
            _bus = bus;
        }

        public bool Logon(string userName, string password, bool persist)
        {
            var userView = _users.GetByEmail(userName);
            if (userView != null && _crypto.GetPasswordHash(password, userView.PasswordSalt) == userView.PasswordHash)
            {
                FormsAuthentication.SetAuthCookie(userName,persist);
                return true;
            }
            //throw new AccessViolationException();
            return false;
        }

         public void Register(string email, string password)
         {
             var cmd = new CreateUser
             {
                 Email = email,
                 Password = password,
             };
             _bus.Send(cmd);
             FormsAuthentication.SetAuthCookie(email,false);
         }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

        public bool ValidateUser(string username, string password)
        {
            var userView = _users.GetByUserName(username);
            return userView != null && _crypto.GetPasswordHash(password, userView.PasswordSalt) == userView.PasswordHash;
        }
    }
}