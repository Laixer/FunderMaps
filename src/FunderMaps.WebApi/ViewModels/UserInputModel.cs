using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.ViewModels
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
        public OrganizationRole Role { get; set; } = OrganizationRole.Reader;
    }
}
