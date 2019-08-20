using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FunderMaps.Extensions
{
    /// <summary>
    /// Extensions to the configuration interface.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get signature key from configuration and convert into security key.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static SymmetricSecurityKey GetJwtSignKey(this IConfiguration configuration)
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SignatureKey"]));

        /// <summary>
        /// Get the issuer from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtIssuer(this IConfiguration configuration)
            => configuration["Jwt:Issuer"];

        /// <summary>
        /// Get the audience from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtAudience(this IConfiguration configuration)
            => configuration["Jwt:Audience"];

        /// <summary>
        /// Get the token expiration time in minutes from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static int GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
            => int.Parse(configuration["Jwt:TokenValidity"]);

        /// <summary>
        /// Get the HTTP_HOST setting if configured.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetDomainHost(this IConfiguration configuration)
            => configuration["HTTP_HOST"];
    }
}
