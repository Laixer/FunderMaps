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
        ///     Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; init; }

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
        ///     Area X min.
        /// </summary>
        public double? XMin { get; set; }

        /// <summary>
        ///     Area Y min.
        /// </summary>
        public double? YMin { get; set; }

        /// <summary>
        ///     Area X max.
        /// </summary>
        public double? XMax { get; set; }

        /// <summary>
        ///     Area Y max.
        /// </summary>
        public double? YMax { get; set; }

        /// <summary>
        ///     Area center X.
        /// </summary>
        public double? CenterX { get; set; }

        /// <summary>
        ///     Area center Y.
        /// </summary>
        public double? CenterY { get; set; }
    }
}
