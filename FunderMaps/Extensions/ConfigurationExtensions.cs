using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Retrieve the configuration option from one of the configuration providers.
        /// </summary>
        /// <param name="configuration">Configuration interface.</param>
        /// <param name="section">Settting section.</param>
        /// <param name="key">Setting key.</param>
        /// <returns>Value or null</returns>
        private static string GetConfigSection(IConfiguration configuration, string section, string key)
        {
            var value = string.Empty;

            // Get option from default configuration
            value = configuration?.GetSection(section)?[key];
            if (value != null)
            {
                return value;
            }

            // Get option from Azure settings
            value = configuration?.GetSection($"APPSETTING_{section}")?[key];
            if (value != null)
            {
                return value;
            }

            // FUTURE: Add any other methodes here

            return value;
        }

        /// <summary>
        /// Get signature key from configuration and convert into security key.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static SymmetricSecurityKey GetJwtSignKey(this IConfiguration configuration)
        {
            var configKey = GetConfigSection(configuration, "Jwt", "SignatureKey");
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
        }

        /// <summary>
        /// Get the issuer from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtIssuer(this IConfiguration configuration)
        {
            return GetConfigSection(configuration, "Jwt", "Issuer");
        }

        /// <summary>
        /// Get the audience from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtAudience(this IConfiguration configuration)
        {
            return GetConfigSection(configuration, "Jwt", "Audience");
        }

        /// <summary>
        /// Get the token expiration time in minutes from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static int GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
        {
            return int.Parse(GetConfigSection(configuration, "Jwt", "TokenValidity"));
        }
    }
}
