using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Generic repository.
    /// </summary>
    /// <typeparam name="TEntity">Derivative of base entity.</typeparam>
    /// <typeparam name="TEntryPrimaryKey">Primary key of entity.</typeparam>
    internal abstract class RepositoryBase<TEntity, TEntryPrimaryKey> : IAsyncRepository<TEntity, TEntryPrimaryKey>
        where TEntity : BaseEntity
        where TEntryPrimaryKey : IEquatable<TEntryPrimaryKey>
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; }

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        protected RepositoryBase(DbProvider dbProvider) => DbProvider = dbProvider;

        /// <summary>
        ///     Runs the SQL command and return an unsigned long value.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <returns>Return value as ulong.</returns>
        public async ValueTask<ulong> ExecuteScalarUnsignedLongCommandAsync(string cmdText)
        {
            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(cmdText, connection);
            return await cmd.ExecuteScalarUnsignedLongAsync().ConfigureAwait(false);
        }

        // FUTURE: Maybe to npgsql specific.
        /// <summary>
        ///     Convert navigation to query.
        /// </summary>
        /// <param name="cmdText">SQL query.</param>
        /// <param name="navigation">Navigation instance of type <see cref="INavigation"/>.</param>
        protected static void ConstructNavigation(ref string cmdText, INavigation navigation)
        {
            // TODO: SECURITY: HACK: This is 100% textbook SQLi.
            if (!string.IsNullOrEmpty(navigation.SortColumn))
            {
                cmdText += $"\r\n ORDER BY {navigation.SortColumn} {(navigation.SortOrder == SortOrder.Ascending ? "ASC" : "DESC")}";
            }
            if (navigation.Offset != 0)
            {
                cmdText += $"\r\n OFFSET {navigation.Offset}";
            }
            if (navigation.Limit != 0)
            {
                cmdText += $"\r\n LIMIT {navigation.Limit}";
            }
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
