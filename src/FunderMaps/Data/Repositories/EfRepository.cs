using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbContext">Database context.</param>
        public EfRepository(TContext dbContext)
        {
            _dbContext = dbContext;
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
