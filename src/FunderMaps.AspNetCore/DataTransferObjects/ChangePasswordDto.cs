using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Change password DTO.
    /// </summary>
    public sealed class ChangePasswordDto
    {
        /// <summary>
        ///     User current password.
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        ///     User new password.
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
    }
}
