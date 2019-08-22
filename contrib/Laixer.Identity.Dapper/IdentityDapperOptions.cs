using Laixer.Identity.Dapper.Database;

namespace Laixer.Identity.Dapper
{
    /// <summary>
    /// Identity Dapper database configuration.
    /// </summary>
    public class IdentityDapperOptions
    {
        /// <summary>
        /// Database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Database schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// User store table.
        /// </summary>
        public string UserTable { get; set; } = "AspNetUsers";

        /// <summary>
        /// Role store table.
        /// </summary>
        public string RoleTable { get; set; } = "AspNetUserRoles";

        /// <summary>
        /// Lets Dapper map record using underscores.
        /// </summary>
        public bool MatchWithUnderscore { get; set; }

        /// <summary>
        /// Database driver for the connection.
        /// </summary>
        public IDatabaseDriver Database { get; set; }
    }
}
