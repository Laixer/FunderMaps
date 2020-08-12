using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Generic repository base.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntryPrimaryKey">Primary key of entity.</typeparam>
    internal abstract class RepositoryBase<TEntity, TEntryPrimaryKey> : DataBase, IAsyncRepository<TEntity, TEntryPrimaryKey>
        where TEntity : BaseEntity
        where TEntryPrimaryKey : IEquatable<TEntryPrimaryKey>
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        protected RepositoryBase(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.GetByIdAsync"/>
        /// </summary>
        public abstract ValueTask<TEntity> GetByIdAsync(TEntryPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.ListAllAsync"/>
        /// </summary>
        public abstract IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.AddAsync"/>
        /// </summary>
        public abstract ValueTask<TEntryPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.UpdateAsync"/>
        /// </summary>
        public abstract ValueTask UpdateAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.DeleteAsync"/>
        /// </summary>
        public abstract ValueTask DeleteAsync(TEntryPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntryPrimaryKey}.CountAsync"/>
        /// </summary>
        public abstract ValueTask<ulong> CountAsync();
    }
}
