using FunderMaps.Core.Interfaces;
using System;

namespace FunderMaps.Data
{
    /// <summary>
    ///     Navigation structure.
    /// </summary>
    public class NavigationImpl<TOffset, TLimit>
        where TOffset : IComparable, IComparable<TOffset>
        where TLimit : IComparable, IComparable<TLimit>
    {
        /// <summary>
        ///     Offset in list.
        /// </summary>
        public TOffset Offset { get; set; }

        /// <summary>
        ///     Limit of items in list.
        /// </summary>
        public TLimit Limit { get; set; }

        /// <summary>
        ///     Column to sort on.
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        ///     Sorting order.
        /// </summary>
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }

    /// <summary>
    /// Navigation structure.
    /// </summary>
    public class Navigation : NavigationImpl<int, int>, INavigation
    {
        /// <summary>
        ///     Return a single row.
        /// </summary>
        public static Navigation SingleRow { get => new Navigation { Limit = 1 }; }

        /// <summary>
        ///     Return all rows.
        /// </summary>
        public static Navigation All { get => new Navigation(); }

        // FUTURE: We can do some sorting.
        /// <summary>
        ///     Return recent rows.
        /// </summary>
        public static Navigation Recent { get => new Navigation { Limit = 5 }; }

        /// <summary>
        ///     Default navigation implementation.
        /// </summary>
        public static Navigation DefaultCollection { get => new Navigation { Limit = 25 }; }
    }
}
