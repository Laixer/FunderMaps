using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Organization user and password DTO.
    /// </summary>
    public record OrganizationUserPasswordDto : OrganizationUserDto
    {
        /// <summary>
        ///     User password.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Password { get; init; }
    }
}
