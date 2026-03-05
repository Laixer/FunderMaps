using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataTransferObjects;

/// <summary>
///     Data transfer object for resetting a new password.
/// </summary>
public sealed record ResetNewPasswordDto
{
    /// <summary>
    ///     User email address.
    /// </summary>
    [Required, EmailAddress]
    public required string Email { get; init; }

    /// <summary>
    ///    User reset key.
    /// </summary>
    [Required]
    public required Guid ResetKey { get; init; }

    /// <summary>
    ///     User new password.
    /// </summary>
    [Required]
    public required string NewPassword { get; init; }
}
