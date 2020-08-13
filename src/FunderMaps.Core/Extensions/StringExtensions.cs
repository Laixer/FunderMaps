using System;

namespace FunderMaps.Webservice.Utility
{
    /// <summary>
    ///     Contains extension functionality for <see cref="string"/> types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/> if <paramref name="str"/>
        ///     is null or empty.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to check</param>
        public static void ThrowIfNullOrEmpty(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
        }
    }
}
