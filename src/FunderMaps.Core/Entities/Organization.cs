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
        ///     Organizatoin registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        ///     Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; set; }

        /// <summary>
        ///     Name on the invoice.
        /// </summary>
        public string InvoiceName { get; set; }

        /// <summary>
        ///     Invoice PB box number.
        /// </summary>
        public string InvoicePoBox { get; set; }

        /// <summary>
        ///     Send invoices to designated email address.
        /// </summary>
        public string InvoiceEmail { get; set; }

        // TODO: Convert to geocoder address?
        // TODO: Drop the PO box?

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
        ///     State or province.
        /// </summary>
        public string HomeState { get; set; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string HomeCountry { get; set; }

        /// <summary>
        ///     Street.
        /// </summary>
        public string PostalStreet { get; set; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? PostalAddressNumber { get; set; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string PostalAddressNumberPostfix { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        public string PostalCity { get; set; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string PostalPostbox { get; set; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string PostalZipcode { get; set; }

        /// <summary>
        ///     State or province.
        /// </summary>
        public string PostalState { get; set; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string PostalCountry { get; set; }

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
