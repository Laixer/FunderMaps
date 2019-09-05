using FunderMaps.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// User input model.
    /// </summary>
    public sealed class UserInputModel
    {
        /// <summary>
        /// User identifier.
        /// </summary>
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [MaxLength(256)]
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
        [MaxLength(256)]
        public OrganizationRole Role { get; set; } = OrganizationRole.Reader;
    }
}
