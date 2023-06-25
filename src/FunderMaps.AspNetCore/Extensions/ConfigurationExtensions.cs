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
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var signatureKey = configuration["Jwt:SignatureKey"] ?? throw new InvalidOperationException("JWT signature key not found in configuration.");
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));
    }

    /// <summary>
    ///     Get the issuer from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static string GetJwtIssuer(this IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer not found in configuration.");
    }

    /// <summary>
    ///     Get the audience from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static string GetJwtAudience(this IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        return configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience not found in configuration.");
    }

    /// <summary>
    ///     Get the token expiration time in minutes from the configuration.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public static TimeSpan GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var tokenValidity = configuration["Jwt:TokenValidity"] ?? throw new InvalidOperationException("JWT token validity not found in configuration.");
        return TimeSpan.FromMinutes(double.Parse(tokenValidity, NumberFormatInfo.InvariantInfo));
    }
}
