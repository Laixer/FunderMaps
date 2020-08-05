using System;

namespace FunderMaps.Core.Types
{
    /// <summary>
    /// Represents a collection of years.
    /// </summary>
    public sealed class Years
    {
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Years() { }

        /// <summary>
        /// Constructor based on two years.
        /// </summary>
        /// <param name="yearFrom"><see cref="Year"/></param>
        /// <param name="yearTo"><see cref="Year"/></param>
        public Years(Year yearFrom, Year yearTo)
        {
            YearFrom = yearFrom ?? throw new ArgumentNullException(nameof(yearFrom));
            YearTo = yearTo ?? throw new ArgumentNullException(nameof(yearTo));
            if (YearFrom.YearValue >= yearTo.YearValue) { throw new ArgumentOutOfRangeException(nameof(yearFrom)); }
        }

        /// <summary>
        /// Static constructor for easy decade creation.
        /// </summary>
        /// <remarks>
        /// The <paramref name="decadeStart"/> must be a multiple of 10. The 
        /// returned <see cref="YearFrom"/> will be created based on this value,
        /// the <see cref="YearTo"/> will be 9 years later.
        /// </remarks>
        /// <param name="decadeStart">Starting year as integer</param>
        /// <returns><see cref="Years"/></returns>
        public static Years FromDecade(int decadeStart)
        {
            if (decadeStart % 10 != 0) { throw new ArgumentOutOfRangeException(nameof(decadeStart)); }
            return new Years
            {
                YearFrom = new Year(decadeStart),
                YearTo = new Year(decadeStart + 9)
            };
        }

        /// <summary>
        /// The first year for this collection of years.
        /// </summary>
        public Year YearFrom { get; set; }

        /// <summary>
        /// The last year in this collection of years.
        /// </summary>
        public Year YearTo { get; set; }
    }
}
