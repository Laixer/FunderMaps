using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AutoMapper
{
    /// <summary>
    ///     Mapper extension methods.
    /// </summary>
    public static class IMapperExtensions
    {
        /// <summary>
        ///     Execute a mapping from the source enumerable to a new destination object.
        /// </summary>
        /// <typeparam name="TDestination">Destination type to create.</typeparam>
        /// <typeparam name="TEntity">Source object entity.</typeparam>
        /// <param name="mapper">Mapper to extend.</param>
        /// <param name="enumerable">Source object to map from.</param>
        /// <returns>Mapped destination object.</returns>
        public static async ValueTask<TDestination> MapAsync<TDestination, TEntity>(this IMapper mapper, IAsyncEnumerable<TEntity> enumerable)
            where TDestination : IEnumerable
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            ICollection<TEntity> entities = new Collection<TEntity>();
            await foreach (var item in enumerable)
            {
                entities.Add(item);
            }

            return mapper.Map<TDestination>(entities);
        }
    }
}
