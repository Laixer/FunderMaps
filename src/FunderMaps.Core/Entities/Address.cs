using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Access entity.
    /// </summary>
    public class Address : BaseEntity
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Building number.
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Postcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Street name.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Address is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// External data source id.
        /// </summary>
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        /// External data source.
        /// </summary>
        [Required]
        public string ExternalSource { get; set; }
    }
}
