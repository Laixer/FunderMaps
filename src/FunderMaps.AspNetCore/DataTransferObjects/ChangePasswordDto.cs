using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Change user password DTO.
    /// </summary>
    public sealed record ChangePasswordDto
    {
        /// <summary>
        ///     User current password.
        /// </summary>
        [Required]
        public string OldPassword { get; init; }

        /// <summary>
        ///     User new password.
        /// </summary>
        [Required]
        public string NewPassword { get; init; }
    }
}
