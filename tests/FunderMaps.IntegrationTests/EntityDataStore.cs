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

        /// <summary>
        ///     Clear all entities from data store.
        /// </summary>
        public void Clear() => Entities.Clear();

        public ulong Count() => (ulong)Entities.Count;

        public bool IsSet => Entities.Count > 0;

        public void Add(IList<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                Entities.Add(entity);
            }
        }
    }
}
