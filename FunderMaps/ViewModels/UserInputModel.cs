using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// User input model used for creation, authentication.
    /// </summary>
    public sealed class UserInputModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Optional user role.
        /// </summary>
        public string Role { get; set; }
    }
}
