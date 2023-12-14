using FunderMaps.Core.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using TokenContext = FunderMaps.Core.Authentication.TokenContext;

namespace FunderMaps.Core.Services;

/// <summary>
///     Jwt bearer token provider.
/// </summary>
public class JwtSecurityTokenService(
    IOptionsMonitor<JwtBearerOptions> options,
    TimeProvider timeProvider)
{
    /// <summary>
    ///     The <see cref="JwtBearerOptions"/> used.
    /// </summary>
    public JwtBearerOptions Options { get; private set; } = options.Get(JwtBearerDefaults.AuthenticationScheme);

    /// <summary>
    ///     Find the first security token handler that can write a token.
    /// </summary>
    private SecurityTokenHandler GetHandler()
    {
        var securityToken = Options.SecurityTokenValidators.FirstOrDefault(securityToken =>
        {
            var securityTokenHandler = securityToken as SecurityTokenHandler;
            return securityTokenHandler?.CanWriteToken ?? false;
        });
        return securityToken as SecurityTokenHandler ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     Generate a <see cref="SecurityToken"/> from a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
    protected SecurityToken GenerateSecurityToken(ClaimsPrincipal principal)
    {
        AuthenticationProperties properties = new();

        JwtTokenValidationParameters JwtTokenValidationParameters = Options.TokenValidationParameters as JwtTokenValidationParameters
            ?? throw new InvalidCastException("Cannot cast TokenValidationParameters to JwtTokenValidationParameters.");
        var issuerSigningKey = JwtTokenValidationParameters.IssuerSigningKey;
        SigningCredentials SigningCredentials = new(issuerSigningKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new(principal.Claims)
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

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

        if (!properties.ExpiresUtc.HasValue && JwtTokenValidationParameters.Valid != TimeSpan.Zero)
        {
            properties.ExpiresUtc = issuedUtc.Add(JwtTokenValidationParameters.Valid);
        }

        return new JwtSecurityToken(
            issuer: JwtTokenValidationParameters.ValidIssuer,
            audience: JwtTokenValidationParameters.ValidAudience,
            claims: claims,
            notBefore: properties.IssuedUtc?.LocalDateTime,
            expires: properties.ExpiresUtc?.LocalDateTime,
            signingCredentials: SigningCredentials);
    }

    /// <summary>
    ///     Generate a <see cref="SecurityToken"/> from a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SecurityToken GetToken(ClaimsPrincipal principal)
        => GenerateSecurityToken(principal);

    /// <summary>
    ///     Generate a <see cref="TokenContext"/> from a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">Claims principal.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public TokenContext GetTokenContext(ClaimsPrincipal principal)
    {
        SecurityToken token = GetToken(principal);
        return new()
        {
            TokenString = GetHandler().WriteToken(token),
            Token = token,
        };
    }
}
