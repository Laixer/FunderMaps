using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FunderMaps.AspNetCore.Authentication;

/// <summary>
///     Security token provider.
/// </summary>
public interface ISecurityTokenProvider
{
    /// <summary>
    ///     Generate token.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
    SecurityToken GetToken(ClaimsPrincipal principal);

    /// <summary>
    ///     Generate token.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    TokenContext GetTokenContext(ClaimsPrincipal principal);
}
