using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Interface for database drivers.
    /// </summary>
    public interface IDatabaseDriver
    {
        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="IDbConnection"/>.</returns>
        IDbConnection GetDbConnection();

        /// <summary>
        /// Return the query repository.
        /// </summary>
        IQueryRepository QueryRepository { get; }
    }
}
