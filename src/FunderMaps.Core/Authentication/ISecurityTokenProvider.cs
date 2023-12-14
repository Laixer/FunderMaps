using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FunderMaps.Core.Authentication;

// TODO: Move to FunderMaps.Core.Interfaces?
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
