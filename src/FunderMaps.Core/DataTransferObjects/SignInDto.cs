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
    public required string Email { get; init; }

    /// <summary>
    ///     User password.
    /// </summary>
    [Required]
    public required string Password { get; init; }
}
