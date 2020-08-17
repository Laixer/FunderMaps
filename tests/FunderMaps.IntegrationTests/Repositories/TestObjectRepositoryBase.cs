using System;

namespace FunderMaps.IntegrationTests.Repositories
{
    /// <summary>
    ///     Base for anything that has an <see cref="ObjectDataStore{TObject}"/>.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public abstract class TestObjectRepositoryBase<TObject>
        where TObject : class
    {
        /// <summary>
        ///     Object data store.
        /// </summary>
        public ObjectDataStore DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestObjectRepositoryBase(ObjectDataStore dataStore) => DataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
    }
}
