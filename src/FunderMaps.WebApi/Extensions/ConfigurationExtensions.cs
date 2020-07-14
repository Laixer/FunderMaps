using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
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
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SignatureKey"]));
        }

        /// <summary>
        /// Get the issuer from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtIssuer(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["Jwt:Issuer"];
        }

        /// <summary>
        /// Get the audience from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetJwtAudience(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["Jwt:Audience"];
        }

        /// <summary>
        /// Get the token expiration time in minutes from the configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static int GetJwtTokenExpirationInMinutes(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return int.Parse(configuration["Jwt:TokenValidity"]);
        }

        /// <summary>
        /// Get the HTTP_HOST setting if configured.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static string GetDomainHost(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["HTTP_HOST"];
        }
    }
}
