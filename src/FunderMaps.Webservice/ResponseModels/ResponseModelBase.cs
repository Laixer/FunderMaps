namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    ///     Empty class serving as base for whatever model we return in <see cref="ResponseWrapper"/>.
    /// </summary>
    public abstract class ResponseModelBase
    {
        /// <summary>
        ///     The internal FunderMaps ID of this building.
        /// </summary>
        public string Id { get; set; }
    }
}
