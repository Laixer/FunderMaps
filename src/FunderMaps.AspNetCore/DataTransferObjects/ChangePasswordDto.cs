using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Change user password DTO.
    /// </summary>
    public sealed class ChangePasswordDto
    {
        /// <summary>
        ///     User current password.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string OldPassword { get; set; }

        /// <summary>
        ///     User new password.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }
    }
}
