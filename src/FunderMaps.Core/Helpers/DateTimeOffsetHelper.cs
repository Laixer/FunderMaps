using System;

namespace FunderMaps.Core.Helpers
{
    // FUTURE: Maybe remove this?
    /// <summary>
    ///     Contains utility functionality for <see cref="DateTimeOffset"/>.
    /// </summary>
    public static class DateTimeOffsetHelper
    {
        /// <summary>
        ///     Creates a new <see cref="DateTimeOffset"/> representing a year.
        /// </summary>
        /// <remarks>
        ///     All but the <see cref="DateTimeOffset.Year"/> properties will be 
        ///     their default value.
        /// </remarks>
        /// <param name="year">Integer representing the year</param>
        /// <returns><see cref="DateTimeOffset"/> representing a year</returns>
        public static DateTimeOffset FromYear(int year) => new(year, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }
}
