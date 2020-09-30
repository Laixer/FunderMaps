using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Generic repository base.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntityPrimaryKey">Primary key of entity.</typeparam>
    internal abstract class RepositoryBase<TEntity, TEntityPrimaryKey> : DataBase, IAsyncRepository<TEntity, TEntityPrimaryKey>
        where TEntity : IdentifiableEntity<TEntity, TEntityPrimaryKey>
        where TEntityPrimaryKey : IEquatable<TEntityPrimaryKey>, IComparable<TEntityPrimaryKey>
    {
        // protected virtual void MapToWriter(DbCommand cmd, TEntity entity)
        // {
        // }

        // protected virtual TEntity MapFromReader(DbDataReader reader)
        // {
        //     return default(TEntity);
        // }

        // TODO: Remove
        /// <summary>
        ///     Runs the SQL command and return an unsigned long value.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <returns>Return value as ulong.</returns>
        public async ValueTask<ulong> ExecuteScalarUnsignedLongCommandAsync(string cmdText)
        {
            await using var context = await DbContextFactory(cmdText);

            return await context.ScalarAsync<ulong>();
        }

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntry, TEntityPrimaryKey}.GetByIdAsync"/>
        /// </summary>
        public abstract ValueTask<TEntity> GetByIdAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.ListAllAsync"/>
        /// </summary>
        public abstract IAsyncEnumerable<TEntity> ListAllAsync(INavigation navigation);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.AddAsync"/>
        /// </summary>
        public abstract ValueTask<TEntityPrimaryKey> AddAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.UpdateAsync"/>
        /// </summary>
        public abstract ValueTask UpdateAsync(TEntity entity);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.DeleteAsync"/>
        /// </summary>
        public abstract ValueTask DeleteAsync(TEntityPrimaryKey id);

        /// <summary>
        ///     <see cref="IAsyncRepository{TEntity, TEntityPrimaryKey}.CountAsync"/>
        /// </summary>
        public abstract ValueTask<ulong> CountAsync();
    }
}
