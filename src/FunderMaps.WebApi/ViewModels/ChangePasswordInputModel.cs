using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// Change password model.
    /// </summary>
    public sealed class ChangePasswordInputModel
    {
        /// <summary>
        /// New password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Old (current) password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
