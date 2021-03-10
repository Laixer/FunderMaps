using FunderMaps.Core.Helpers;
using System;

namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Represents a collection of years.
    /// </summary>
    public sealed class Years
    {
        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Years()
        {
        }

        /// <summary>
        ///     Static constructor for easy decade creation.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="decadeStart"/> must be a multiple of 10. The 
        ///     returned <see cref="YearFrom"/> will be created based on this value,
        ///     the <see cref="YearTo"/> will be 9 years later.
        /// </remarks>
        /// <param name="decadeStart">Starting year as integer</param>
        /// <returns><see cref="Years"/></returns>
        public static Years FromDecade(int decadeStart)
        {
            if (decadeStart % 10 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decadeStart));
            }

            return new Years
            {
                YearFrom = DateTimeOffsetHelper.FromYear(decadeStart),
                YearTo = DateTimeOffsetHelper.FromYear(decadeStart + 9),
            };
        }

        /// <summary>
        ///     The first year for this collection of years.
        /// </summary>
        public DateTimeOffset YearFrom { get; set; }

        /// <summary>
        ///     The last year in this collection of years.
        /// </summary>
        public DateTimeOffset YearTo { get; set; }
    }
}
