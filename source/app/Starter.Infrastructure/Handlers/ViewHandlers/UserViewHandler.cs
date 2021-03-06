﻿using DQF.Databases;
using DQF.Domain.Aggregates.Site.Events;
using DQF.Domain.Aggregates.User.Events;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Extensions;
using DQF.Views;
using Uniform;

namespace DQF.Handlers.ViewHandlers
{
    public class UserViewHandler : MessageHandler
    {
        private readonly ViewDatabase _db;
        private readonly IDocumentCollection<UserView> _users;

        public UserViewHandler(ViewDatabase db)
        {
            _db = db;
            _users = db.Users;

            Handle((UserCreated e)=> _users.Save(new UserView
            {
                Id = e.Id,
                UserName = e.UserName,
                Email = e.Email.ToLowerInvariant(),
                PasswordHash = e.PasswordHash,
                PasswordSalt = e.PasswordSalt,
                CreationDate = e.CreationDate,
                Role = e.Role,
                PhoneNumber = e.PhoneNumber
            }));
            Handle((PasswordChanged e) => _users.Update(e.Id, u =>
            {
                u.PasswordHash = e.PasswordHash;
                u.PasswordSalt = e.PasswordSalt;
            }));
            Handle((UserDetailsUpdated e) => _users.Update(e.Id, u =>
            {
                u.UserName = e.UserName;
                u.PhoneNumber = e.PhoneNumber;
                u.AdditionalEmails = e.AdditionalEmails;
            }));
            Handle((UserDeleted e) => _users.Delete(e.Id));
        }
    }
}