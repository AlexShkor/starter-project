using System.Collections.Generic;
using DQF.Domain.Aggregates.User;
using DQF.Helpers;
using DQF.Platform.ViewServices;
using MongoDB.Driver.Builders;
using DQF.Databases;
using DQF.Platform.Mongo;
using DQF.Views;

namespace DQF.ViewServices
{
    public class UsersViewService : ViewService<UserView>
    {
        private readonly CryptographicHelper _crypto;

        public UsersViewService(MongoViewDatabase database, CryptographicHelper crypto)
            : base(database)
        {
            _crypto = crypto;
        }

        protected override ReadonlyMongoCollection<UserView> Items
        {
            get { return Database.Users; }
        }

        public UserView GetByUserName(string userName)
        {
            return Items.FindOne(Query<UserView>.EQ(x => x.UserName, userName));
        }

        public UserView GetByEmail(string email)
        {
            return GetById(_crypto.GetMd5Hash(email));
        }

        public IEnumerable<UserView> GetNotAdmins()
        {
            return Items.Find(Query<UserView>.NE(x => x.Role, UserRoleEnum.GlobalAdmin));
        }
    }
}