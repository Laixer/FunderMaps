using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.IntegrationTests
{
    // FUTURE Duplicate functionality with <see cref="EntityDataStore{TEntity}"/>.
    /// <summary>
    ///     Data store for objects.
    /// </summary>
    /// <typeparam name="TObject"><see cref="class"/></typeparam>
    public class ObjectDataStore
    {
        /// <summary>
        ///     All stored objects in this store.
        /// </summary>
        public IList<object> Objects { get; set; } = new List<object>();

        /// <summary>
        ///     All stored objects in this store of type <typeparamref name="TObject"/>.
        /// </summary>
        /// <typeparam name="TObject">Object type.</typeparam>
        /// <returns>Collection of <typeparamref name="TObject"/></returns>
        public IEnumerable<TObject> GetObjectsFromType<TObject>()
            where TObject : class
            => Objects.Where(x => x.GetType() == typeof(TObject)).Cast<TObject>();

        /// <summary>
        ///     Add objects to the data store.
        /// </summary>
        /// <param name="obj">objects object to add.</param>
        /// <returns>Inserted objects.</returns>
        public object Add(object obj)
        {
            Objects.Add(obj);
            return obj;
        }

        /// <summary>
        ///     Add all objects to the data store.
        /// </summary>
        /// <param name="objList">Object list to add.</param>
        public void AddList(IEnumerable<object> objList)
        {
            foreach (var obj in objList)
            {
                Objects.Add(obj);
            }
        }

        /// <summary>
        ///     Clear all objects from data store.
        /// </summary>
        public void ClearAll() => Objects.Clear();

        /// <summary>
        ///     Clear all objects of type <typeparamref name="TObject"/>
        ///     and add a single item.
        /// </summary>
        /// <typeparam name="TObject">Object type</typeparam>
        /// <param name="obj"><typeparamref name="TObject"/></param>
        public void ClearByTypeAndAddSingle<TObject>(TObject obj)
            where TObject : class
        {
            ClearByType<TObject>();
            Objects.Add(obj);
        }

        /// <summary>
        ///     Clear all objects of type <typeparamref name="TObject"/>
        ///     and add a list of items.
        /// </summary>
        /// <typeparam name="TObject">Object type</typeparam>
        /// <param name="objectList"><typeparamref name="TObject"/> list</param>
        public void ClearByTypeAndAddList<TObject>(IEnumerable<TObject> objectList)
            where TObject : class
        {
            ClearByType<TObject>();
            foreach (var obj in objectList)
            {
                Objects.Add(obj);
            }
        }

        /// <summary>
        ///     Gets the number of objects in the data store.
        /// </summary>
        public ulong Count => (ulong)Objects.Count;

        /// <summary>
        ///     True if there are objects in the data store.
        /// </summary>
        public bool IsSet => Objects.Count > 0;

        /// <summary>
        ///     Clear all objects of a given type from the data store.
        /// </summary>
        private void ClearByType<TObject>()
            where TObject : class
        {
            var leftover = new List<object>();
            foreach (var item in Objects)
            {
                if (item.GetType() != typeof(TObject))
                {
                    leftover.Add(item);
                }
            }
            Objects = leftover;
        }
    }
}
