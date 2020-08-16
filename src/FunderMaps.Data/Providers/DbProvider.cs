using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Providers
{
    // TODO: This is not testable. We want unit tests for this part.

    /// <summary>
    ///     Database provider.
    /// </summary>
    internal abstract class DbProvider
    {
        private readonly DbProviderOptions _options;

        public string ConnectionString { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="options">Configuration options.</param>
        public DbProvider(IConfiguration configuration, IOptions<DbProviderOptions> options)
        {
            _options = options?.Value;
            ConnectionString = configuration.GetConnectionString(_options.ConnectionStringName);
        }

        /// <summary>
        ///     Create a new connection instance.
        /// </summary>
        /// <returns><see cref="DbConnection"/> instance.</returns>
        public abstract DbConnection ConnectionScope();

        /// <summary>
        ///     Open database connection.
        /// </summary>
        /// <returns>See <see cref="DbConnection"/>.</returns>
        public virtual async Task<DbConnection> OpenConnectionScopeAsync()
        {
            var connection = ConnectionScope();
            await connection.OpenAsync();
            return connection;
        }

        /// <summary>
        ///     Create command on the database connection.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        /// <param name="connection">Database connection, see <see cref="DbConnection"/>.</param>
        /// <returns>See <see cref="DbCommand"/>.</returns>
        public virtual DbCommand CreateCommand(string cmdText, DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var cmd = connection.CreateCommand();
            cmd.CommandText = cmdText;
            return cmd;
        }
    }
}
