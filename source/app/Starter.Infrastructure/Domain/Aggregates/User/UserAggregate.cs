using System;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Domain.Aggregates.User.Events;
using DQF.Platform.Domain;
using DQF.Platform.Extensions;

namespace DQF.Domain.Aggregates.User
{
    public class UserAggregate : Aggregate<UserState>
    {
        public void Create(string id, string userName, string passwordHash, string passwordSalt, string email, string phone, UserRoleEnum role)
        {
            if (State.Id.HasValue())
            {
                throw new InvalidOperationException("User with same ID already exist.");
            }
            Apply(new UserCreated
            {
                Id = id,
                UserName = userName,
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreationDate = DateTime.Now,
                Role = role,
                PhoneNumber = phone
            });
        }

        public void ChangePassword(ChangePassword c)
        {
            Apply(new PasswordChanged
            {
                Id = State.Id,
                PasswordHash = c.PasswordHash,
                PasswordSalt = c.PasswordSalt,
                Date = c.Date,
                WasChangedByAdmin = c.IsChangedByAdmin,
            });
        }

        public void Delete(string deteledByUser)
        {
            if (State.Role == UserRoleEnum.GlobalAdmin)
            {
                throw new InvalidOperationException("EDPM admin can't be deleted.");
            }
            Apply(new UserDeleted
            {
                Id = State.Id,
                DeletedByUserId = deteledByUser
            });
        }

        public void UpdateDetails(UpdateUserDetails c)
        {
            Apply(new UserDetailsUpdated
            {
                Id = c.Id,
                UserName = c.UserName,
                PhoneNumber = c.PhoneNumber,
                AdditionalEmails = c.AdditionalEmails
            });
        }
    }
}