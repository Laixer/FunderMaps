using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     User signin result DTO.
/// </summary>
public record SignInSecurityTokenDto
{
    /// <summary>
    ///     Authentication token identifier.
    /// </summary>
    [Required]
    public string? Id { get; init; }

    /// <summary>
    ///     Authentication issuer.
    /// </summary>
    [Required]
    public string? Issuer { get; init; }

    /// <summary>
    ///     Authentication token.
    /// </summary>
    [Required]
    public string? Token { get; init; }

    /// <summary>
    ///     Authentication token valid from datetime.
    /// </summary>
    public DateTime ValidFrom { get; init; }

    /// <summary>
    ///     Authentication token valid until datetime.
    /// </summary>
    public DateTime ValidTo { get; init; }
}
