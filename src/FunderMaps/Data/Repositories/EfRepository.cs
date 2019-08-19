using System;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Providers;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="TContext">Database context.</typeparam>
    /// <typeparam name="TEntry">Derivative of base entity.</typeparam>
    public class EfRepository<TContext, TEntry> : IAsyncRepository<TEntry>
        where TContext : DbContext
        where TEntry : BaseEntity
    {
        protected readonly TContext _dbContext;
        protected readonly DbProvider _dbProvider;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        /// <param name="dbProvider">Database provider.</param>
        public EfRepository(TContext dbContext, DbProvider dbProvider = null) // TODO: Remove the default value
        {
            _dbContext = dbContext;
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// Runs the SQL command and creates the connection if necessary.
        /// </summary>
        /// <typeparam name="TReturn">Query return type.</typeparam>
        /// <param name="action">SQL query.</param>
        /// <param name="_connection">Optional database connection</param>
        /// <returns>Awaitable with return value.</returns>
        protected async Task<TReturn> RunSqlCommand<TReturn>(Func<IDbConnection, Task<TReturn>> action, IDbConnection _connection = null)
        {
            // Run with existing connection.
            if (_connection != null)
            {
                return await action(_connection);
            }

            // Run in new scope.
            using (var connection = _dbProvider.ConnectionScope())
            {
                return await action(connection);
            }
        }

        /// <summary>
        /// Return entity by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Entity.</returns>
        public virtual Task<TEntry> GetByIdAsync(int id)
        {
            return _dbContext.Set<TEntry>().FindAsync(id);
        }

        public virtual async Task<IReadOnlyList<TEntry>> ListAllAsync()
        {
            return await _dbContext.Set<TEntry>().ToListAsync();
        }

        public virtual async Task<TEntry> AddAsync(TEntry entity)
        {
            _dbContext.Set<TEntry>().Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual Task UpdateAsync(TEntry entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task DeleteAsync(TEntry entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;

            return _dbContext.SaveChangesAsync();
        }

        public virtual Task<int> CountAsync()
        {
            return _dbContext.Set<TEntry>().AsNoTracking().CountAsync();
        }
    }
}
