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
        /// Wrapper around GetConnectionString.
        /// </summary>
        /// <param name="configuration">The configuration to extend.</param>
        /// <param name="name">Connection name.</param>
        /// <returns>Connection string or null.</returns>
        public static string GetConnectionStringFallback(this IConfiguration configuration, string name)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var connectionString = configuration.GetConnectionString(name);

            // FUTURE: There is no support for PostgreSQL in the configuration as of yet.
            //         Solution is at Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider.cs
            //         Proposal: https://github.com/aspnet/Extensions/pull/1695

            if (connectionString != null) { return connectionString; }
            connectionString = configuration.GetValue<string>($"POSTGRESQLCONNSTR_{name}");

            if (connectionString != null) { return connectionString; }
            connectionString = configuration.GetValue<string>($"MYSQLCONNSTR_{name}");

            if (connectionString != null) { return connectionString; }
            connectionString = configuration.GetValue<string>($"SQLAZURECONNSTR_{name}");

            if (connectionString != null) { return connectionString; }
            connectionString = configuration.GetValue<string>($"SQLCONNSTR_{name}");

            if (connectionString != null) { return connectionString; }
            connectionString = configuration.GetValue<string>($"CUSTOMCONNSTR_{name}");

            return connectionString;
        }

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
