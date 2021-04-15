using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Base class for analysis endpoint responses.
    /// </summary>
    public record AnalysisBaseDto
    {
        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string BuildingId { get; set; }

        /// <summary>
        ///     Building external identifier.
        /// </summary>
        public string ExternalBuildingId { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; set; }

        /// <summary>
        ///     Address external identifier.
        /// </summary>
        public string ExternalAddressId { get; set; }

        /// <summary>
        ///     Neighborhood identifier.
        /// </summary>
        [Geocoder]
        public string NeighborhoodId { get; set; }
    }
}
