namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response model containing the amount of buildings which have a given
    /// <see cref="FoundationType"/>.
    /// </summary>
    public sealed class FoundationTypePairResponseModel
    {
        /// <summary>
        /// The type of foundation.
        /// </summary>
        public string FoundationType { get; set; }

        /// <summary>
        /// The total amount of buildings having this foundation type.
        /// </summary>
        public uint TotalCount { get; set; }
    }
}
