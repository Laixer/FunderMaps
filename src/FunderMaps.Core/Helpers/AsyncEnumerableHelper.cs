namespace FunderMaps.Core.Helpers
{
    /// <summary>
    ///     Helpers for async enumerable operations.
    /// </summary>
    public static class AsyncEnumerableHelper
    {
        /// <summary>
        ///     Return single item as <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <param name="item">Single item to yield return.</param>
        /// <returns>A <see cref="IAsyncEnumerable{T}"/>.</returns>
        public static async IAsyncEnumerable<T> AsEnumerable<T>(T item)
        {
            yield return await new ValueTask<T>(item);
            yield break;
        }
    }
}
