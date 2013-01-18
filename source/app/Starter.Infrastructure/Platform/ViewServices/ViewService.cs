using System.Collections.Generic;
using DQF.Databases;
using DQF.Platform.Mongo;

namespace DQF.Platform.ViewServices
{
    public abstract class ViewService<T>
    {
        protected readonly MongoViewDatabase Database;

        protected ViewService(MongoViewDatabase database)
        {
            Database = database;
        }

        protected abstract ReadonlyMongoCollection<T> Items { get; }

        public virtual T GetById(string id)
        {
            return Items.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Items.FindAll();
        }
    }
}
