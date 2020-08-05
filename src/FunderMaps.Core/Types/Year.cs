using System;

namespace FunderMaps.Core.Types
{
    /// <summary>
    /// Represents a year.
    /// TODO Is this the way to go?
    /// </summary>
    public sealed class Year
    {
        /// <summary>
        /// Creates a <see cref="Year"/> from a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="dto"><see cref="DateTimeOffset"/></param>
        public Year(DateTimeOffset dto) => YearValue = dto.Year;

        /// <summary>
        /// Creates a <see cref="Year"/> from an integer.
        /// </summary>
        /// <param name="thisYear">Integer representing this year</param>
        public Year(int thisYear) => YearValue = thisYear;

        /// <summary>
        /// Represents this year.
        /// </summary>
        public int YearValue { get; set; }
    }
}
