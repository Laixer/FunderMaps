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
    public string Email { get; init; } = default!;

    /// <summary>
    ///    User reset key.
    /// </summary>
    [Required]
    public Guid ResetKey { get; init; } = default!;

    /// <summary>
    ///     User new password.
    /// </summary>
    [Required]
    public string NewPassword { get; init; } = default!;
}
