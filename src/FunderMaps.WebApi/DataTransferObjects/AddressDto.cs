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
        [Address]
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
        ///     Address is active or not.
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        ///     External data source id.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string ExternalId { get; set; }

        /// <summary>
        ///     External data source.
        /// </summary>
        [Required]
        public string ExternalSource { get; set; }
    }
}
