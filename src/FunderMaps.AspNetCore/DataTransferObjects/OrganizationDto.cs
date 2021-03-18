using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Organization DTO.
    /// </summary>
    public sealed record OrganizationDto
    {
        /// <summary>
        ///     Organization identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     Gets or sets the name for the organization.
        /// </summary>
        [Required]
        public string Name { get; init; }

        /// <summary>
        ///     Gets or sets the email address for the organization.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; init; }

        /// <summary>
        ///     Phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; init; }

        /// <summary>
        ///     Organizatoin registration number.
        /// </summary>
        public string RegistrationNumber { get; init; }

        /// <summary>
        ///     Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; init; }

        /// <summary>
        ///     Name on the invoice.
        /// </summary>
        public string InvoiceName { get; init; }

        /// <summary>
        ///     Invoice PB box number.
        /// </summary>
        public string InvoicePoBox { get; init; }

        /// <summary>
        ///     Send invoices to designated email address.
        /// </summary>
        public string InvoiceEmail { get; init; }

        // FUTURE: Convert to geocoder address?

        /// <summary>
        ///     Street.
        /// </summary>
        public string HomeStreet { get; init; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? HomeAddressNumber { get; init; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string HomeAddressNumberPostfix { get; init; }

        /// <summary>
        ///     City.
        /// </summary>
        public string HomeCity { get; init; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string HomePostbox { get; init; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string HomeZipcode { get; init; }

        /// <summary>
        ///     State or province.
        /// </summary>
        public string HomeState { get; init; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string HomeCountry { get; init; }

        /// <summary>
        ///     Street.
        /// </summary>
        public string PostalStreet { get; init; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? PostalAddressNumber { get; init; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string PostalAddressNumberPostfix { get; init; }

        /// <summary>
        ///     City.
        /// </summary>
        public string PostalCity { get; init; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string PostalPostbox { get; init; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string PostalZipcode { get; init; }

        /// <summary>
        ///     State or province.
        /// </summary>
        public string PostalState { get; init; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string PostalCountry { get; init; }
    }
}
