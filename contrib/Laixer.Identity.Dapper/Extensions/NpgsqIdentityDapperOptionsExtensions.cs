namespace Laixer.Identity.Dapper.Extensions
{
    public static class NpgsqIdentityDapperOptionsExtensions
    {
        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString)
        {
            options.ConnectionString = connectionString;

            options.Database = new Database.Npgsql(options);

            return options;
        }

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="schema">Database schema.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString, string schema)
        {
            options.ConnectionString = connectionString;
            options.Schema = schema;

            options.Database = new Database.Npgsql(options);

            return options;
        }

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="schema">Database schema.</param>
        /// <param name="userTable">Databse user store table.</param>
        /// <param name="roleTable">Databse role store table.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString, string schema, string userTable, string roleTable)
        {
            options.ConnectionString = connectionString;
            options.Schema = schema;
            options.UserTable = userTable;
            options.RoleTable = roleTable;

            options.Database = new Database.Npgsql(options);

            return options;
        }
    }
}
