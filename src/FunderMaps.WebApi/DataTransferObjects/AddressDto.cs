using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Address DTO.
    /// </summary>
    public sealed class AddressDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Geocoder]
        public string Id { get; set; }

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
    }
}
