using System;
using DQF.Domain.Aggregates.User;
using DQF.Platform.Extensions;
using DQF.Views;

namespace DQF.Web.Models.User
{
    public class UserListItem
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CreationDate { get; set; }
        public UserRoleEnum Role { get; set; }
        public string RoleString 
        {
            get
            {
                return Role.GetDescription();
            }
        }

        public bool CanBeDeleted
        {
            get { return Role != UserRoleEnum.GlobalAdmin; }
        }

        public UserListItem(UserView user)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            CreationDate = user.CreationDate.ToShortDateString();
            Role = user.Role;
        }
    }
}