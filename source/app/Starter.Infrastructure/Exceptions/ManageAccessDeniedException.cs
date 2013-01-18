using System;
using DQF.Common.Account;

namespace DQF.Exceptions
{
    public class ManageAccessDeniedException: Exception
    {
        public ManageAccessDeniedException(UserIdentity user):
            base(string.Format("User {0} with ID: {1} denied acces to manage entity.", user.Email, user.UserId))
        {
        }
    }
}