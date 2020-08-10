using FunderMaps.Core.Interfaces;
using System;

namespace FunderMaps.Core.Extensions
{
    /// <summary>
    ///     Contains extension funcationality for <see cref="INavigation"/>.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        ///     Validates an <see cref="INavigation"/> object.
        /// </summary>
        /// <param name="navigation"><see cref="INavigation"/></param>
        public static void Validate(this INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            if (navigation.Limit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(navigation));
            }

            if (navigation.Offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(navigation));
            }
        }
    }
}
