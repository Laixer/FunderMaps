using FunderMaps.Core.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Address with building DTO.
    /// </summary>
    public sealed record AddressBuildingDto
    {
        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; init; }

        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string BuildingId { get; init; }

        /// <summary>
        ///     Building number.
        /// </summary>
        [Required]
        public string BuildingNumber { get; init; }

        /// <summary>
        ///     Postcode.
        /// </summary>
        public string PostalCode { get; init; }

        /// <summary>
        ///     Street name.
        /// </summary>
        [Required]
        public string Street { get; init; }

        /// <summary>
        ///     City.
        /// </summary>
        [Required]
        public string City { get; init; }

        /// <summary>
        ///     Building Built year.
        /// </summary>
        public DateTime? BuiltYear { get; init; }

        /// <summary>
        ///     Building Geometry.
        /// </summary>
        [Required]
        public string BuildingGeometry { get; init; }
    }
}
