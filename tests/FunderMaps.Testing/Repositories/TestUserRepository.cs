﻿using FunderMaps.Core;
using FunderMaps.Core.Entities;
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

        public virtual async Task<User> AddGetAsync(User entity)
        {
            Guid primaryKey = await AddAsync(entity);
            return await GetByIdAsync(primaryKey);
        }

        protected virtual UserRecord FindEntityById(Guid id)
            => DataStore.ItemList.FirstOrDefault(e => e.User.Id == id);

        protected virtual int FindIndexById(Guid id)
            => DataStore.ItemList.IndexOf(FindEntityById(id));

        /// <summary>
        ///     Add entity to data store.
        /// </summary>
        /// <param name="entity"></param>
        public Task<Guid> AddAsync(User entity)
        {
            entity.Id = Guid.NewGuid();
            DataStore.Add(new UserRecord
            {
                User = entity
            });
            return Task.FromResult<Guid>(entity.Id);
        }

        /// <summary>
        ///     Count items in datastore.
        /// </summary>
        /// <returns>Number of items in data store.</returns>
        public Task<long> CountAsync()
            => Task.FromResult<long>(DataStore.Count);

        public Task BumpAccessFailed(Guid id)
        {
            FindEntityById(id).AccessFailedCount++;
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Remove entity from data store.
        /// </summary>
        /// <param name="id"></param>
        public virtual Task DeleteAsync(Guid id)
        {
            DataStore.ItemList.Remove(FindEntityById(id));
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Return entity from data store by id.
        /// </summary>
        /// <param name="id">Entity primary key.</param>
        /// <returns>Instance of <see cref="TEntity"/>.</returns>
        public Task<User> GetByIdAsync(Guid id)
        {
            return Task.FromResult<User>(FindEntityById(id).User);
        }

        /// <summary>
        ///     Return all items in the data store.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>See <see cref="IAsyncEnumerable<TEntity>"/>.</returns>
        public IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
        {
            return Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Select(s => s.User), navigation));
        }

        /// <summary>
        ///     Update entity in data store.
        /// </summary>
        /// <param name="entity"></param>
        public Task UpdateAsync(User entity)
        {
            DataStore.ItemList[FindIndexById(entity.Id)].User = entity;
            return Task.CompletedTask;
        }

        public Task<uint> GetAccessFailedCountAsync(Guid id)
        {
            return Task.FromResult<uint>(FindEntityById(id).AccessFailedCount);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return Task.FromResult<User>(DataStore.ItemList.FirstOrDefault(e => e.User.Email == email).User);
        }

        public Task<DateTime?> GetLastLoginAsync(Guid id)
        {
            return Task.FromResult<DateTime?>(FindEntityById(id).LastLogin);
        }

        public Task<uint> GetLoginCountAsync(Guid id)
        {
            return Task.FromResult<uint>(FindEntityById(id).AccessCount);
        }

        public Task<string> GetPasswordHashAsync(Guid id)
        {
            return Task.FromResult<string>(FindEntityById(id).Password);
        }

        public Task<bool> IsLockedOutAsync(Guid id)
        {
            return Task.FromResult<bool>(FindEntityById(id).IsLockedOut);
        }

        public Task RegisterAccess(Guid id)
        {
            UserRecord userRecord = FindEntityById(id);
            userRecord.AccessCount++;
            userRecord.LastLogin = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task ResetAccessFailed(Guid id)
        {
            FindEntityById(id).AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Guid id, string passwordHash)
        {
            FindEntityById(id).Password = passwordHash;
            return Task.CompletedTask;
        }
    }
}
