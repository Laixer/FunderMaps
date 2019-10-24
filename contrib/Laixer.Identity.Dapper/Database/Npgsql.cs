using Npgsql;
using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Driver for PostgreSQL driver.
    /// </summary>
    internal partial class Npgsql : IDatabaseDriver
    {
        private readonly IdentityDapperOptions _options;

        /// <summary>
        /// Return the query repository.
        /// </summary>
        public IQueryRepository QueryRepository { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance.</param>
        public Npgsql(IdentityDapperOptions options, ICustomQueryRepository customQueryRepository = null, IQueryRepository queryRepository = null)
        {
            _options = options;
            QueryRepository = queryRepository ?? new Internal.DefaultQueryRepository();

            // Set the default schema for PostgreSQL
            if (string.IsNullOrEmpty(_options.Schema))
            {
                _options.Schema = "public";
            }

            // Prepare queries for PostgreSQL driver.
            PrepareQueries();

            // Let caller change query properties
            if (customQueryRepository != null)
            {
                customQueryRepository.Configure(QueryRepository);
            }
        }

        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="NpgsqlConnection"/>.</returns>
        public IDbConnection GetDbConnection() => new NpgsqlConnection(_options.ConnectionString);
    }
}
