using Npgsql;
using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Driver for PostgreSQL driver.
    /// </summary>
    internal class Npgsql : IDatabaseDriver
    {
        private readonly IdentityDapperOptions _options;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance.</param>
        public Npgsql(IdentityDapperOptions options)
        {
            _options = options;

            // Set the default schema for PostgreSQL
            if (string.IsNullOrEmpty(_options.Schema))
            {
                _options.Schema = "public";
            }
        }

        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="NpgsqlConnection"/>.</returns>
        public IDbConnection GetDbConnection() => new NpgsqlConnection(_options.ConnectionString);
    }
}
