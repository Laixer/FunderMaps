using FunderMaps.Core.Entities;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests
{
    public class EntityDataStore<TEntity>
        where TEntity : BaseEntity
    {
        public IList<TEntity> Entities { get; set; } = new List<TEntity>();

        public TEntity Add(TEntity entity)
        {
            Entities.Add(entity);
            return entity;
        }

        public ulong Count()
        {
            return (ulong)Entities.Count;
        }

        public void Add(IList<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                Entities.Add(entity);
            }
        }
    }
}
