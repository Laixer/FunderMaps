using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Tdm.Extensions;
{
    internal static class FunctionSharedStateExtensions
    {
        /// <summary>
        /// Store object as shared state.
        /// </summary>
        /// <param name="state">Extends <see cref="FunctionSharedState"/>.</param>
        /// <param name="name">File name.</param>
        /// <param name="obj"></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static Task UpdateAsync(this FunctionSharedState state, string name, object obj, CancellationToken cancellationToken = default)
            => state.UpdateAsync(name, JsonConvert.SerializeObject(obj), cancellationToken);

        /// <summary>
        /// Get shared state object.
        /// </summary>
        /// <typeparam name="T">Type of return object.</typeparam>
        /// <param name="state">Extends <see cref="FunctionSharedState"/>.</param>
        /// <param name="name">File name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Return shared state as <typeparamref name="T"/>.</returns>
        public static async Task<T> GetAsync<T>(this FunctionSharedState state, string name, CancellationToken cancellationToken = default)
            => JsonConvert.DeserializeObject<T>(await state.GetAsync(name, cancellationToken).ConfigureAwait(false));
    }
}
