using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace FunderMaps.Providers
{
    /// <summary>
    /// Database provider.
    /// </summary>
    public class DbProvider
    {
        private readonly DbProviderOptions _options;
        private readonly string connectionString;

        /// <summary>
        /// Application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Static initializer.
        /// </summary>
        static DbProvider()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="options">Configuration options.</param>
        public DbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        {
            Configuration = configuration;
            _options = options?.Value;
            connectionString = Configuration.GetConnectionString(_options.ConnectionStringName);

            // FUTURE: There is no support for PostgreSQL in the configuration as of yet.
            //         Solution is at Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider.cs
            //         Proposal: https://github.com/aspnet/Extensions/pull/1695

            if (connectionString != null) { return; }
            connectionString = Configuration.GetValue<string>($"POSTGRESQLCONNSTR_{_options.ConnectionStringName}");

            if (connectionString != null) { return; }
            connectionString = Configuration.GetValue<string>($"MYSQLCONNSTR_{_options.ConnectionStringName}");

            if (connectionString != null) { return; }
            connectionString = Configuration.GetValue<string>($"SQLAZURECONNSTR_{_options.ConnectionStringName}");

            if (connectionString != null) { return; }
            connectionString = Configuration.GetValue<string>($"SQLCONNSTR_{_options.ConnectionStringName}");

            if (connectionString != null) { return; }
            connectionString = Configuration.GetValue<string>($"CUSTOMCONNSTR_{_options.ConnectionStringName}");
        }

        /// <summary>
        /// Create a new connection instance.
        /// </summary>
        /// <returns><see cref="IDbConnection"/> instance.</returns>
        public IDbConnection ConnectionScope() => new NpgsqlConnection(connectionString);
    }
}
