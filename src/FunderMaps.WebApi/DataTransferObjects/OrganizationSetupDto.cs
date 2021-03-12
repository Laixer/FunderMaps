using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Organization setup DTO.
    /// </summary>
    public sealed class OrganizationSetupDto
    {
        /// <summary>
        ///     User email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     User password.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
