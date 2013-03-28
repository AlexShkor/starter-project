using System;
using DQF.Domain.Aggregates.User.Commands;
using DQF.Helpers;
using DQF.Platform.Dispatching;
using DQF.Platform.Dispatching.Interfaces;
using DQF.Platform.Domain.Interfaces;

namespace DQF.Domain.Aggregates.User
{
    public class UserApplicationService : MessageHandler
    {
        private readonly IRepository<UserAggregate> _repository;
        private readonly CryptographicHelper _crypto;

        public UserApplicationService(IRepository<UserAggregate> repository, CryptographicHelper crypto)
        {
            _repository = repository;
            _crypto = crypto;
            Handle((CreateUser c)=>
            {
                var salt = _crypto.GenerateSalt();
                var id = _crypto.GetMd5Hash(c.Email);
                _repository.Perform(id, user => user.Create(
                    id,
                    c.UserName,
                    _crypto.GetPasswordHash(c.Password, salt),
                    salt,
                    c.Email,
                    c.Phone, c.Role));
            });
            HandleCommand<ChangePassword>((c, user) => user.ChangePassword(c));
            HandleCommand<DeleteUser>((c, user) => user.Delete(c.DeletedByUserId));
            HandleCommand<UpdateUserDetails>((c, user) => user.UpdateDetails(c));
        }

        private void HandleCommand<TMessage>(Action<TMessage, UserAggregate> handler)where TMessage: UserCommand
        {
            Handle<TMessage>((e)=> _repository.Perform(e.UserId,user=> handler(e,user)));
        }
    }
}