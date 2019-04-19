using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Extensions
{
    public static class DbSetExtensions
    {
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
