using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Access entity.
    /// </summary>
    public class Address : BaseEntity
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

        // TODO: This is a type.
        /// <summary>
        ///     External data source.
        /// </summary>
        [Required]
        public string ExternalSource { get; set; }
    }
}
