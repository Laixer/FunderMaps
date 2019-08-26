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

        string CreateAsync { get; }
        string FindByEmailAsync { get; }
        string GetPasswordHashAsync { get; }
    }
}
