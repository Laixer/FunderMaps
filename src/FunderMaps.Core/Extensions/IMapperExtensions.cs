using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
        /// <param name="token">The cancellation instruction.</param>
        /// <returns>Mapped destination object.</returns>
        public static async Task<TDestination> MapAsync<TDestination, TEntity>(
            this IMapper mapper,
            IAsyncEnumerable<TEntity> enumerable,
            CancellationToken token = default)
            where TDestination : IEnumerable
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            Collection<TEntity> entities = new();
            await foreach (var item in enumerable.WithCancellation(token))
            {
                entities.Add(item);
            }

            return mapper.Map<TDestination>(entities);
        }
    }
}
