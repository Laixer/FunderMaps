using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     User signin DTO.
    /// </summary>
    public class SignInDto
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
