using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core;

namespace FunderMaps.Testing
{
    /// <summary>
    ///     Helper class.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        ///     Return <see cref="IEnumerable"/> as <see cref="IAsyncEnumerable"/>.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="list">Input enumerable.</param>
        /// <returns>Instance of <see cref="IAsyncEnumerable"/>.</returns>
        public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(IEnumerable<T> list)
        {
            await Task.CompletedTask;
            foreach (var item in list)
            {
                yield return item;
            }
        }

        /// <summary>
        ///     Apply navigation rules to <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="list">Input enumerable.</param>
        /// <param name="navigation">Navigation rules.</param>
        /// <returns></returns>
        public static IEnumerable<T> ApplyNavigation<T>(IEnumerable<T> list, Navigation navigation)
        {
            if (navigation.Offset > 0)
            {
                list = list.Skip(navigation.Offset);
            }
            if (navigation.Limit > 0)
            {
                list = list.Take(navigation.Limit);
            }

            return list;
        }
    }
}
