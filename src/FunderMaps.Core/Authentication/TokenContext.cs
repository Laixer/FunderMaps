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
    public string TokenString { get; init; } = default!;

    /// <summary>
    ///     Security token.
    /// </summary>
    public SecurityToken Token { get; init; } = default!;
}
