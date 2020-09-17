using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.InputModels
{
    /// <summary>
    ///     User signin DTO.
    /// </summary>
    public sealed class SignInInputModel
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
