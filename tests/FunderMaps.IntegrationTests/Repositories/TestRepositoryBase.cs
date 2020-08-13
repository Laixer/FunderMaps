using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    /// <summary>
    ///     The test repository base should be able to mock an entire
    ///     datastore repository.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TEntryPrimaryKey">Entity primary key.</typeparam>
    public abstract class TestRepositoryBase<TEntity, TEntryPrimaryKey> : IAsyncRepository<TEntity, TEntryPrimaryKey>
        where TEntity : BaseEntity
        where TEntryPrimaryKey : IEquatable<TEntryPrimaryKey>
    {
        protected Func<TEntity, TEntryPrimaryKey> EntityPrimaryKey { get; }

        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public EntityDataStore<TEntity> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestRepositoryBase(EntityDataStore<TEntity> dataStore, Func<TEntity, TEntryPrimaryKey> entryPrimaryKey)
        {
            DataStore = dataStore;
            EntityPrimaryKey = entryPrimaryKey;
        }

        protected virtual TEntity FindEntityById(TEntryPrimaryKey id)
            => DataStore.Entities.FirstOrDefault(e => EntityPrimaryKey(e).Equals(id));

        protected virtual int FindIndexById(TEntryPrimaryKey id)
            => DataStore.Entities.IndexOf(DataStore.Entities.FirstOrDefault(e => EntityPrimaryKey(e).Equals(id)));

        /// <summary>
        ///     Add entity to data store.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValueTask<TEntryPrimaryKey> AddAsync(TEntity entity)
        {
            DataStore.Add(entity);
            return new ValueTask<TEntryPrimaryKey>(EntityPrimaryKey(entity));
        }

        /// <summary>
        ///     Count items in datastore.
        /// </summary>
        /// <returns>Number of items in data store.</returns>
        public virtual ValueTask<ulong> CountAsync()
            => new ValueTask<ulong>(DataStore.Count);

        /// <summary>
        ///     Remove entity from data store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ValueTask DeleteAsync(TEntryPrimaryKey id)
        {
            DataStore.Entities.Remove(FindEntityById(id));
            return new ValueTask();
        }

        /// <summary>
        ///     Return entity from data store by id.
        /// </summary>
        /// <param name="id">Entity primary key.</param>
        /// <returns>Instance of <see cref="TEntity"/>.</returns>
        public virtual ValueTask<TEntity> GetByIdAsync(TEntryPrimaryKey id)
            => new ValueTask<TEntity>(FindEntityById(id));

        /// <summary>
        ///     Return all items in the data store.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>See <see cref="IAsyncEnumerable<TEntity>"/>.</returns>
        public virtual IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation)
            => Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.Entities, navigation));

        /// <summary>
        ///     Update entity in data store.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValueTask UpdateAsync(TEntity entity)
        {
            DataStore.Entities[FindIndexById(EntityPrimaryKey(entity))] = entity;
            return new ValueTask();
        }
    }
}
