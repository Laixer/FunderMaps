namespace FunderMaps.Webservice.Core.Models.Building
{
    /// <summary>
    /// Represents a single building in the FunderMaps data store.
    /// </summary>
    public class BuildingBase<TProduct> : ProductModelBase
    {
        /// <summary>
        /// Internal FunderMaps id of this building.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// External id of this building.
        /// TODO With datasource?
        /// </summary>
        public string ExternalId { get; set; }
    }
}
