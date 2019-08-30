using System;
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
        public Guid Id { get; set; }

        /// <summary>
        /// Street name.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string StreetName { get; set; }

        /// <summary>
        /// Building number.
        /// </summary>
        [Required]
        public short BuildingNumber { get; set; }

        /// <summary>
        /// Building number suffix.
        /// </summary>
        [MaxLength(8)]
        public string BuildingNumberSuffix { get; set; }
    }
}
