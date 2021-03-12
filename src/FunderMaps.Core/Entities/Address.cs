using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Access entity.
    /// </summary>
    public sealed class Address : IdentifiableEntity<Address, string>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Address()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Required, Geocoder]
        public string Id { get; set; }

        /// <summary>
        ///     Building number.
        /// </summary>
        [Required]
        public string BuildingNumber { get; set; }

        /// <summary>
        ///     Postcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        ///     Street name.
        /// </summary>
        [Required]
        public string Street { get; set; }

        /// <summary>
        ///     Address is active or not.
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        ///     External data source id.
        /// </summary>
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        ///     External data source.
        /// </summary>
        [Required]
        public ExternalDataSource ExternalSource { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        ///     Building identifier.
        /// </summary>
        [Geocoder]
        public string BuildingId { get; set; }

        /// <summary>
        ///     Full address.
        /// </summary>
        public string FullAddress => $"{Street} {BuildingNumber}, {City}";

        // TODO: Obsolete
        /// <summary>
        ///     Building identifier.
        /// </summary>
        public Building BuildingNavigation { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing user.</returns>
        public override string ToString() => Id;
    }
}
