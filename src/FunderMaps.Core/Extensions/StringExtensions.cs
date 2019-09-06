using System.Linq;

namespace FunderMaps.Core.Extensions
{
    /// <summary>
    /// String Extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert camelCase into snake casing.
        /// </summary>
        /// <param name="str">Input string to extend.</param>
        /// <returns>Converted string.</returns>
        public static string ToSnakeCase(this string str)
            => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x)
            ? "_" + x.ToString()
            : x.ToString())).ToLower().Trim();
    }
}
