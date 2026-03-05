using FunderMaps.Core.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TokenContext = FunderMaps.Core.Authentication.TokenContext;

namespace FunderMaps.Core.Services;

/// <summary>
///     Jwt bearer token provider.
/// </summary>
public class JwtSecurityTokenService(IOptionsMonitor<JwtBearerOptions> options, TimeProvider timeProvider)
{
    /// <summary>
    ///     The <see cref="JwtBearerOptions"/> used.
    /// </summary>
    public JwtBearerOptions Options { get; private set; } = options.Get(JwtBearerDefaults.AuthenticationScheme);

    /// <summary>
    ///     Generate a <see cref="SecurityToken"/> from a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
    protected SecurityToken GenerateSecurityToken(ClaimsPrincipal principal)
    {
        var properties = new AuthenticationProperties();

        var jwtParams = Options.TokenValidationParameters as JwtTokenValidationParameters
            ?? throw new InvalidCastException("Cannot cast TokenValidationParameters to JwtTokenValidationParameters.");
        var signingCredentials = new SigningCredentials(jwtParams.IssuerSigningKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = [..principal.Claims, new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())];

        var nameClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.Ordinal));
        if (nameClaim is not null)
        {
            claims.Add(new(JwtRegisteredClaimNames.Sub, nameClaim.Value));
        }

        DateTimeOffset issuedUtc;
        if (properties.IssuedUtc.HasValue)
        {
            issuedUtc = properties.IssuedUtc.Value;
        }
        else
        {
            issuedUtc = timeProvider.GetUtcNow();
            properties.IssuedUtc = issuedUtc;
        }

        if (!properties.ExpiresUtc.HasValue && jwtParams.Valid != TimeSpan.Zero)
        {
            properties.ExpiresUtc = issuedUtc.Add(jwtParams.Valid);
        }

        return new JwtSecurityToken(
            issuer: jwtParams.ValidIssuer,
            audience: jwtParams.ValidAudience,
            claims: claims,
            notBefore: properties.IssuedUtc?.LocalDateTime,
            expires: properties.ExpiresUtc?.LocalDateTime,
            signingCredentials: signingCredentials);
    }

    /// <summary>
    ///     Generate a <see cref="TokenContext"/> from a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public TokenContext GetTokenContext(ClaimsPrincipal principal)
    {
        SecurityToken token = GenerateSecurityToken(principal);
        return new()
        {
            TokenString = new JwtSecurityTokenHandler().WriteToken(token),
            Token = token,
        };
    }
}
