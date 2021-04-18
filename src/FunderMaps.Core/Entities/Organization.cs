using FunderMaps.Core.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Organization entity.
    /// </summary>
    public sealed class Organization : IdentifiableEntity<Organization, Guid>, ITenant
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Organization()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the name for the organization.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the email address for the organization.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; set; }

        /// <summary>
        ///     Street.
        /// </summary>
        public string HomeStreet { get; set; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? HomeAddressNumber { get; set; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string HomeAddressNumberPostfix { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        public string HomeCity { get; set; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string HomePostbox { get; set; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string HomeZipcode { get; set; }

        /// <summary>
        ///     Geospatial account area.
        /// </summary>
        public SpatialBox Area { get; set; }

        /// <summary>
        ///     Geospatial account area center.
        /// </summary>
        public SpatialPoint Center { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing organization.</returns>
        public override string ToString() => Name;

        /// <summary>
        ///     Initialize property defaults.
        /// </summary>
        public override void InitializeDefaults()
        {
            Id = Guid.Empty;
        }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public override void InitializeDefaults(Organization other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
            Name = other.Name;
            Email = other.Email;
        }
    }
}
