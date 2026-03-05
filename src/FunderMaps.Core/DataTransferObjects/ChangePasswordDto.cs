using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataTransferObjects;

/// <summary>
///     Change user password DTO.
/// </summary>
public sealed record ChangePasswordDto
{
    /// <summary>
    ///     User current password.
    /// </summary>
    [Required]
    public required string OldPassword { get; init; }

    /// <summary>
    ///     User new password.
    /// </summary>
    [Required]
    public required string NewPassword { get; init; }
}
