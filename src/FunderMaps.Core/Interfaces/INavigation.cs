namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Sorting order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        ///     Ascending sorting.
        /// </summary>
        Ascending,

        /// <summary>
        ///     Descending sorting.
        /// </summary>
        Descending,
    }

    /// <summary>
    ///     Navigation structure.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        ///     Offset in list.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        ///     Limit of items in list.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        ///     Column to sort on.
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        ///     Sorting order.
        /// </summary>
        public SortOrder SortOrder { get; set; }
    }
}
