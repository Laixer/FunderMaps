using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataTransferObjects;

/// <summary>
///     User signin DTO.
/// </summary>
public sealed record SignInDto
{
    /// <summary>
    ///     User email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; init; } = default!;

    /// <summary>
    ///     User password.
    /// </summary>
    [Required]
    public string Password { get; init; } = default!;
}
