using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class UserRecord
    {
        public User User { get; set; }
        public uint AccessFailedCount { get; set; }
        public uint AccessCount { get; set; }
        public DateTime LastLogin { get; set; }
        public string Password { get; set; }
        public bool IsLockedOut { get; set; }
    }

    public class TestUserRepository : IUserRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public DataStore<UserRecord> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestUserRepository(DataStore<UserRecord> dataStore)
        {
            DataStore = dataStore;
        }

        protected virtual UserRecord FindEntityById(Guid id)
            => DataStore.ItemList.FirstOrDefault(e => e.User.Id == id);

        protected virtual int FindIndexById(Guid id)
            => DataStore.ItemList.IndexOf(FindEntityById(id));

        /// <summary>
        ///     Add entity to data store.
        /// </summary>
        /// <param name="entity"></param>
        public ValueTask<Guid> AddAsync(User entity)
        {
            entity.Id = Guid.NewGuid();
            DataStore.Add(new UserRecord
            {
                User = entity
            });
            return new ValueTask<Guid>(entity.Id);
        }

        /// <summary>
        ///     Count items in datastore.
        /// </summary>
        /// <returns>Number of items in data store.</returns>
        public ValueTask<ulong> CountAsync()
            => new ValueTask<ulong>((ulong)DataStore.Count);

        public ValueTask BumpAccessFailed(Guid id)
        {
            FindEntityById(id).AccessFailedCount++;
            return new ValueTask();
        }

        /// <summary>
        ///     Remove entity from data store.
        /// </summary>
        /// <param name="id"></param>
        public virtual ValueTask DeleteAsync(Guid id)
        {
            DataStore.ItemList.Remove(FindEntityById(id));
            return new ValueTask();
        }

        /// <summary>
        ///     Return entity from data store by id.
        /// </summary>
        /// <param name="id">Entity primary key.</param>
        /// <returns>Instance of <see cref="TEntity"/>.</returns>
        public ValueTask<User> GetByIdAsync(Guid id)
        {
            return new ValueTask<User>(FindEntityById(id).User);
        }

        /// <summary>
        ///     Return all items in the data store.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>See <see cref="IAsyncEnumerable<TEntity>"/>.</returns>
        public IAsyncEnumerable<User> ListAllAsync(INavigation navigation)
        {
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Select(s => s.User), navigation));
        }

        /// <summary>
        ///     Update entity in data store.
        /// </summary>
        /// <param name="entity"></param>
        public ValueTask UpdateAsync(User entity)
        {
            DataStore.ItemList[FindIndexById(entity.Id)].User = entity;
            return new ValueTask();
        }

        public ValueTask<uint> GetAccessFailedCountAsync(Guid id)
        {
            return new ValueTask<uint>(FindEntityById(id).AccessFailedCount);
        }

        public ValueTask<User> GetByEmailAsync(string email)
        {
            return new ValueTask<User>(DataStore.ItemList.FirstOrDefault(e => e.User.Email == email).User);
        }

        public ValueTask<DateTime?> GetLastLoginAsync(Guid id)
        {
            return new ValueTask<DateTime?>(FindEntityById(id).LastLogin);
        }

        public ValueTask<uint> GetLoginCountAsync(Guid id)
        {
            return new ValueTask<uint>(FindEntityById(id).AccessCount);
        }

        public ValueTask<string> GetPasswordHashAsync(Guid id)
        {
            return new ValueTask<string>(FindEntityById(id).Password);
        }

        public ValueTask<bool> IsLockedOutAsync(Guid id)
        {
            return new ValueTask<bool>(FindEntityById(id).IsLockedOut);
        }

        public ValueTask RegisterAccess(Guid id)
        {
            UserRecord userRecord = FindEntityById(id);
            userRecord.AccessCount++;
            userRecord.LastLogin = DateTime.Now;
            return new ValueTask();
        }

        public ValueTask ResetAccessFailed(Guid id)
        {
            FindEntityById(id).AccessFailedCount = 0;
            return new ValueTask();
        }

        public ValueTask SetPasswordHashAsync(Guid id, string passwordHash)
        {
            FindEntityById(id).Password = passwordHash;
            return new ValueTask();
        }
    }
}
