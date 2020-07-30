using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Building entity.
    /// </summary>
    public sealed class Building : BaseEntity
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Built year.
        /// </summary>
        public DateTime BuiltYear { get; set; }

        /// <summary>
        ///     Building is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Required]
        public string Address { get; set; }

        // TODO: Type
        /// <summary>
        ///     External data source id.
        /// </summary>
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        ///     External data source.
        /// </summary>
        [Required]
        public string ExternalSource { get; set; }

        /// <summary>
        ///     Address object.
        /// </summary>
        public Address AddressNavigation { get; set; }
    }
}
