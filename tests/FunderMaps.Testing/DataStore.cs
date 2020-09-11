using System.Collections.Generic;

namespace FunderMaps.Testing
{
    /// <summary>
    ///     Memory data store.
    /// </summary>
    /// <typeparam name="TItem">Item object.</typeparam>
    public class DataStore<TItem>
        where TItem : class
    {
        /// <summary>
        ///     Datastore objects.
        /// </summary>
        public IList<TItem> ItemList { get; private set; } = new List<TItem>();

        /// <summary>
        ///     Gets the number of items in the data store.
        /// </summary>
        public int Count => ItemList.Count;

        /// <summary>
        ///     Add item to the data store.
        /// </summary>
        /// <param name="item">Object to add.</param>
        /// <returns>Inserted item.</returns>
        public TItem Add(TItem item)
        {
            ItemList.Add(item);
            return item;
        }

        /// <summary>
        ///     Add items to the data store.
        /// </summary>
        /// <param name="item">Item list to add.</param>
        public void Reset(TItem item)
        {
            ItemList = new List<TItem>() { item };
        }

        /// <summary>
        ///     Replace item store with list.
        /// </summary>
        /// <remarks>
        ///     Copy the list into a local list so we
        ///     can operate on a dynamic enumerable.
        /// </remarks>
        /// <param name="item">Item list to add.</param>
        public void Reset(IEnumerable<TItem> list)
        {
            ItemList = new List<TItem>(list);
        }
    }
}
