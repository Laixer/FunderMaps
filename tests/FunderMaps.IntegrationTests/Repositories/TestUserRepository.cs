using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class UserRecord : BaseEntity
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
        public EntityDataStore<UserRecord> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestUserRepository(EntityDataStore<UserRecord> dataStore)
        {
            DataStore = dataStore;
        }

        protected virtual UserRecord FindEntityById(Guid id)
            => DataStore.Entities.FirstOrDefault(e => e.User.Id == id);

        protected virtual int FindIndexById(Guid id)
            => DataStore.Entities.IndexOf(FindEntityById(id));

        /// <summary>
        ///     Add entity to data store.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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
            => new ValueTask<ulong>(DataStore.Count());

        public ValueTask BumpAccessFailed(User entity)
        {
            FindEntityById(entity.Id).AccessFailedCount++;
            return new ValueTask();
        }

        /// <summary>
        ///     Remove entity from data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ValueTask DeleteAsync(Guid id)
        {
            DataStore.Entities.Remove(FindEntityById(id));
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
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.Entities.Select(s => s.User), navigation));
        }

        /// <summary>
        ///     Update entity in data store.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ValueTask UpdateAsync(User entity)
        {
            DataStore.Entities[FindIndexById(entity.Id)].User = entity;
            return new ValueTask();
        }

        public ValueTask<uint> GetAccessFailedCountAsync(User entity)
        {
            return new ValueTask<uint>(FindEntityById(entity.Id).AccessFailedCount);
        }

        public ValueTask<User> GetByEmailAsync(string email)
        {
            return new ValueTask<User>(DataStore.Entities.FirstOrDefault(e => e.User.Email == email).User);
        }

        public ValueTask<DateTime?> GetLastLoginAsync(User entity)
        {
            return new ValueTask<DateTime?>(FindEntityById(entity.Id).LastLogin);
        }

        public ValueTask<uint?> GetLoginCountAsync(User entity)
        {
            return new ValueTask<uint?>(FindEntityById(entity.Id).AccessCount);
        }

        public ValueTask<string> GetPasswordHashAsync(User entity)
        {
            return new ValueTask<string>(FindEntityById(entity.Id).Password);
        }

        public ValueTask<bool> IsLockedOutAsync(User entity)
        {
            return new ValueTask<bool>(FindEntityById(entity.Id).IsLockedOut);
        }

        public ValueTask RegisterAccess(User entity)
        {
            UserRecord userRecord = FindEntityById(entity.Id);
            userRecord.AccessCount++;
            userRecord.LastLogin = DateTime.Now;
            return new ValueTask();
        }

        public ValueTask ResetAccessFailed(User entity)
        {
            FindEntityById(entity.Id).AccessFailedCount = 0;
            return new ValueTask();
        }

        public ValueTask SetPasswordHashAsync(User entity, string passwordHash)
        {
            FindEntityById(entity.Id).Password = passwordHash;
            return new ValueTask();
        }
    }
}
