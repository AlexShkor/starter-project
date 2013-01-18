using System;
using DQF.Common.Account;

namespace DQF.Exceptions
{
    public class ViewAccessDeniedException: Exception
    {
        public ViewAccessDeniedException(UserIdentity user)
            : base(string.Format("User {0} with ID: {1} denied acces to view entity.", user.Email, user.UserId))
        {
        }
    }
}