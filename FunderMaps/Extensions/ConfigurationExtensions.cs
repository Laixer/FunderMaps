using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get signature key from configuration and convert into security key.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static SymmetricSecurityKey GetJwtSignKey(this IConfiguration configuration)
        {
            var configKey = configuration?.GetSection("Jwt")?["SignatureKey"];

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
        }

        /// <summary>
        /// Get the issuer from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtIssuer(this IConfiguration configuration)
        {
            return configuration?.GetSection("Jwt")?["Issuer"];
        }

        /// <summary>
        /// Get the audience from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtAudience(this IConfiguration configuration)
        {
            return configuration?.GetSection("Jwt")?["Audience"];
        }

        /// <summary>
        /// Get the token expiration time in minutes from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static int GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
        {
            return int.Parse(configuration?.GetSection("Jwt")?["TokenValidity"]);
        }
    }
}
