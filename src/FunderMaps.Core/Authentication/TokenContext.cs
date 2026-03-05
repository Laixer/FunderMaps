using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.Core.Authentication;

/// <summary>
///     Security token context.
/// </summary>
public record TokenContext
{
    /// <summary>
    ///     Security token as string.
    /// </summary>
    public required string TokenString { get; init; }

    /// <summary>
    ///     Security token.
    /// </summary>
    public required SecurityToken Token { get; init; }
}
