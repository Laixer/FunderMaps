using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Change password DTO.
    /// </summary>
    public class ChangePasswordDto
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
