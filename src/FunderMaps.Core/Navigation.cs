namespace FunderMaps.Core
{
    /// <summary>
    ///     Sorting order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        ///     Ascending sorting.
        /// </summary>
        Ascending = 0,

        /// <summary>
        ///     Descending sorting.
        /// </summary>
        Descending = 1,
    }

    /// <summary>
    ///     Navigation structure.
    /// </summary>
    public class Navigation
    {
        /// <summary>
        ///     Offset in list.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     Limit of items in list.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///     Column to sort on.
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        ///     Sorting order.
        /// </summary>
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

        /// <summary>
        ///     Return a single row.
        /// </summary>
        public static Navigation SingleRow { get => new() { Limit = 1 }; }

        /// <summary>
        ///     Return all rows.
        /// </summary>
        public static Navigation All { get => new(); }

        // FUTURE: We can do some sorting.
        /// <summary>
        ///     Return recent rows.
        /// </summary>
        public static Navigation Recent { get => new() { Limit = 5 }; }

        /// <summary>
        ///     Default navigation implementation.
        /// </summary>
        public static Navigation DefaultCollection { get => new() { Limit = 25 }; }
    }
}
