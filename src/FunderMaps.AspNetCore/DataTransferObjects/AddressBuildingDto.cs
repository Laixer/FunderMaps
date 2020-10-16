using FunderMaps.Core.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Address with building DTO.
    /// </summary>
    public sealed class AddressBuildingDto
    {
        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Geocoder]
        public string AddressId { get; set; }

        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string BuildingId { get; set; }

        /// <summary>
        ///     Building number.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string BuildingNumber { get; set; }

        /// <summary>
        ///     Postcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///     Street name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Street { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        ///     Building Built year.
        /// </summary>
        public DateTime? BuiltYear { get; set; }

        /// <summary>
        ///     Building Geometry.
        /// </summary>
        [Required]
        public string BuildingGeometry { get; set; }
    }
}
