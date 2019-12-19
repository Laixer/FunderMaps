using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Organization entity.
    /// </summary>
    public class Organization : BaseEntity
    {
        /// <summary>
        /// Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name for the organization.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for the organization.
        /// </summary>
        [IgnoreDataMember]
        public string NormalizedName { get; set; }

        /// <summary>
        /// Gets or sets the email address for the organization.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Organizatoin registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Is the default organization.
        /// </summary>
        [IgnoreDataMember]
        public bool? IsDefault { get; set; }

        /// <summary>
        /// Is organization validated.
        /// </summary>
        [IgnoreDataMember]
        public bool? IsValidated { get; set; }

        /// <summary>
        /// Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; set; }

        /// <summary>
        /// Name on the invoice.
        /// </summary>
        public string InvoiceName { get; set; }

        /// <summary>
        /// Invoice PB box number.
        /// </summary>
        public string InvoicePONumber { get; set; }

        /// <summary>
        /// Send invoices to designated email address.
        /// </summary>
        public string InvoiceEmail { get; set; }

        /// <summary>
        /// Street.
        /// </summary>
        public string HomeStreet { get; set; }

        /// <summary>
        /// Street number.
        /// </summary>
        public int? HomeAddressNumber { get; set; }

        /// <summary>
        /// Address number postfix.
        /// </summary>
        public string HomeAddressNumberPostfix { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string HomeCity { get; set; }

        /// <summary>
        /// Postbox number.
        /// </summary>
        public string HomePostbox { get; set; }

        /// <summary>
        /// Zipcode or postcode.
        /// </summary>
        public string HomeZipcode { get; set; }

        /// <summary>
        /// State or province.
        /// </summary>
        public string HomeState { get; set; }

        /// <summary>
        /// Country.
        /// </summary>
        public string HomeCountry { get; set; }

        /// <summary>
        /// Street.
        /// </summary>
        public string PostalStreet { get; set; }

        /// <summary>
        /// Street number.
        /// </summary>
        public int? PostalAddressNumber { get; set; }

        /// <summary>
        /// Address number postfix.
        /// </summary>
        public string PostalAddressNumberPostfix { get; set; }

        /// <summary>
        /// City.
        /// </summary>
        public string PostalCity { get; set; }

        /// <summary>
        /// Postbox number.
        /// </summary>
        public string PostalPostbox { get; set; }

        /// <summary>
        /// Zipcode or postcode.
        /// </summary>
        public string PostalZipcode { get; set; }

        /// <summary>
        /// State or province.
        /// </summary>
        public string PostalState { get; set; }

        /// <summary>
        /// Country.
        /// </summary>
        public string PostalCountry { get; set; }

        /// <summary>
        /// Center of fence X.
        /// </summary>
        public double CenterX { get; set; }

        /// <summary>
        /// Center of fence Y.
        /// </summary>
        public double CenterY { get; set; }

        /// <summary>
        /// Print object as name.
        /// </summary>
        /// <returns>String representing organization name.</returns>
        public override string ToString() => Name;
    }
}
