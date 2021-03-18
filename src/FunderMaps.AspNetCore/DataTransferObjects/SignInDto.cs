using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     User signin DTO.
    /// </summary>
    public sealed record SignInDto
    {
        /// <summary>
        ///     User email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; init; }

        /// <summary>
        ///     User password.
        /// </summary>
        [Required]
        public string Password { get; init; }
    }
}
