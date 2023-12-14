using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

namespace FunderMaps.Extensions;

/// <summary>
///     Extensions to the configuration interface.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Get signature key from configuration and convert into security key.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static SymmetricSecurityKey GetJwtSigningKey(this IConfiguration configuration)
    {
        var signatureKey = configuration["Jwt:SignatureKey"] ?? throw new InvalidOperationException("JWT signature key not found in configuration.");
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));
    }

    /// <summary>
    ///     Get the token expiration time in minutes from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static TimeSpan GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
    {
        var tokenValidity = configuration["Jwt:TokenValidity"] ?? throw new InvalidOperationException("JWT token validity not found in configuration.");
        return TimeSpan.FromMinutes(double.Parse(tokenValidity, NumberFormatInfo.InvariantInfo));
    }
}
