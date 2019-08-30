namespace FunderMaps.Core.Repositories
{
    /// <summary>
    /// Navigation structure.
    /// </summary>
    public class Navigation
    {
        /// <summary>
        /// Offset in list.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Limit of items in list.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="offset">Offset in list.</param>
        /// <param name="limit">Limit of items in list.</param>
        public Navigation(int offset, int limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }
}
