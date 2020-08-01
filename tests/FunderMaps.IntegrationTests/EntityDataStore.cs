using FunderMaps.Core.Entities;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests
{
    public class EntityDataStore<TEntity>
        where TEntity : BaseEntity
    {
        public IList<TEntity> Entities { get; set; } = new List<TEntity>();
    }
}
