using System;

namespace FunderMaps.Core.Repositories
{
    /// <summary>
    /// Navigation structure.
    /// </summary>
    public class NavigationImpl<TOffset, TLimit>
        where TOffset : IComparable, IComparable<TOffset>
        where TLimit : IComparable, IComparable<TLimit>
    {
        /// <summary>
        /// Offset in list.
        /// </summary>
        public TOffset Offset { get; set; }

        /// <summary>
        /// Limit of items in list.
        /// </summary>
        public TLimit Limit { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="offset">Offset in list.</param>
        /// <param name="limit">Limit of items in list.</param>
        public NavigationImpl(TOffset offset, TLimit limit)
        {
            Offset = offset;
            Limit = limit;
        }
    }

    /// <summary>
    /// Navigation structure.
    /// </summary>
    public class Navigation : NavigationImpl<int, int>
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="offset">Offset in list.</param>
        /// <param name="limit">Limit of items in list.</param>
        public Navigation(int offset, int limit)
            : base(offset, limit)
        {
        }
    }
}
