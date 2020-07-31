using System;

namespace FunderMaps.Webservice.Utility
{
    /// <summary>
    /// Contains extension functionality for <see cref="string"/> types.
    /// TODO Move to some utility project.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="s"/>
        /// is null or empty.
        /// </summary>
        /// <param name="s">The <see cref="string"/> to check</param>
        public static void ThrowIfNullOrEmpty(this string s)
        {
            if (s == null) { throw new ArgumentNullException(nameof(s)); }
            if (string.IsNullOrEmpty(s)) { throw new ArgumentNullException(nameof(s)); }
        }
    }
}
