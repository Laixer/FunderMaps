using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataTransferObjects;

/// <summary>
///     Reset user password DTO.
/// </summary>
public sealed record ResetPasswordDto
{
    /// <summary>
    ///     User email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; init; } = default!;
}
