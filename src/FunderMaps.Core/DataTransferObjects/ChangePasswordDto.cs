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
    public string OldPassword { get; init; } = default!;

    /// <summary>
    ///     User new password.
    /// </summary>
    [Required]
    public string NewPassword { get; init; } = default!;
}
