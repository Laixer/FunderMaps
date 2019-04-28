using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Extensions
{
    /// <summary>
    /// Database context extensions.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Get an entity by predicate or insert the provided entity as new.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbset">Database context entity.</param>
        /// <param name="entity">Entity for insertion.</param>
        /// <param name="predicate">Predicate selector for existing entity.</param>
        /// <returns></returns>
        public static async Task<TEntity> GetOrAddAsync<TEntity>(this DbSet<TEntity> dbset, TEntity entity, System.Linq.Expressions.Expression<System.Func<TEntity, bool>> predicate)
            where TEntity : class, new()
        {
            if (entity == null) { return null; }

            var _entity = await dbset.FirstOrDefaultAsync(predicate);
            if (_entity == null)
            {
                await dbset.AddAsync(entity);
                _entity = entity;
            }

            return _entity;
        }
    }
}
