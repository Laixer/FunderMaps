using FunderMaps.Core.Entities;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Fakes a entity data store.
    /// </summary>
    /// <typeparam name="TEntity">Entity object.</typeparam>
    public class EntityDataStore<TEntity>
        where TEntity : BaseEntity<TEntity>
    {
        public IList<TEntity> Entities { get; private set; } = new List<TEntity>();

        /// <summary>
        ///     Add entity to the data store.
        /// </summary>
        /// <param name="entity">Entity object to add.</param>
        /// <returns>Inserted entity.</returns>
        public TEntity Add(TEntity entity)
        {
            Entities.Add(entity);
            return entity;
        }

        /// <summary>
        ///     Add all entities to the data store.
        /// </summary>
        /// <param name="entityList">Entity list to add.</param>
        public void Add(IEnumerable<TEntity> entityList)
        {
            foreach (var entity in entityList)
            {
                Entities.Add(entity);
            }
        }

        /// <summary>
        ///     Add all entities to the data store.
        /// </summary>
        /// <param name="entityList">Entity list to add.</param>
        public void Reset(TEntity entity)
        {
            Entities = new List<TEntity>() { entity };
        }

        /// <summary>
        ///     Add all entities to the data store.
        /// </summary>
        /// <param name="entityList">Entity list to add.</param>
        public void Reset(IEnumerable<TEntity> entityList)
        {
            Entities = new List<TEntity>(entityList);
        }

        /// <summary>
        ///     Clear all entities from data store.
        /// </summary>
        public void Clear() => Entities.Clear();

        /// <summary>
        ///     Gets the number of entities in the data store.
        /// </summary>
        public ulong Count => (ulong)Entities.Count;

        /// <summary>
        ///     True if there are entities in the data store.
        /// </summary>
        public bool IsSet => Entities.Count > 0;
    }
}
