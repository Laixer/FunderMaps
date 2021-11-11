using System.Collections;
using System.Collections.ObjectModel;

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
        /// <remarks>
        ///     <para>
        ///         An asynchronous list is not necessarily faster than its synchronous counterpart.
        ///         A loop which yield each element should be used in the context of a deferred item fetch.
        ///         Such data sources include databases with cursor support and disk-IO operations. When
        ///         the entire data set is known in advance an asynchronous list may provide an overhead.
        ///     </para>
        ///     <para>
        ///         NOTE: *Never* call this method multi-threaded.
        ///     </para>
        /// </remarks>
        /// <typeparam name="TDestination">Destination type to create.</typeparam>
        /// <typeparam name="TEntity">Source object entity.</typeparam>
        /// <param name="mapper">Mapper to extend.</param>
        /// <param name="enumerable">Asynchronous source object to map from.</param>
        /// <param name="token">The cancellation instruction.</param>
        /// <returns>Mapped destination object as <typeparamref name="TDestination"/>.</returns>
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
