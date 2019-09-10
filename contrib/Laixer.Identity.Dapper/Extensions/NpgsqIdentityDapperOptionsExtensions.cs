namespace Laixer.Identity.Dapper.Extensions
{
    /// <summary>
    /// NpgsqIdentityDapperOptions extensions.
    /// </summary>
    public static class NpgsqIdentityDapperOptionsExtensions
    {
        // FUTURE: Pass existing connection to the driver.

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString)
            => options.UseNpgsql(connectionString, null, null, null, null);

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="customQueryRepository">Custom query repository configuration.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString, ICustomQueryRepository customQueryRepository)
            => options.UseNpgsql(connectionString, null, null, null, customQueryRepository);

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <typeparam name="TCustomDatabaseQuery">Custom query repository configuration.</typeparam>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql<TCustomDatabaseQuery>(this IdentityDapperOptions options, string connectionString)
            where TCustomDatabaseQuery : ICustomQueryRepository, new()
            => options.UseNpgsql(connectionString, null, null, null, new TCustomDatabaseQuery());

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="schema">Database schema.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options, string connectionString, string schema)
            => options.UseNpgsql(connectionString, schema, null, null, null);

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
            => options.UseNpgsql(connectionString, schema, userTable, roleTable, null);

        /// <summary>
        /// Adds Npgsql as database driver.
        /// </summary>
        /// <param name="options">The <see cref="IdentityDapperOptions"/> instance this method extends.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="schema">Database schema.</param>
        /// <param name="userTable">Databse user store table.</param>
        /// <param name="roleTable">Databse role store table.</param>
        /// <param name="customQueryRepository">Custom query repository configuration.</param>
        /// <returns>The <see cref="IdentityDapperOptions"/> instance this method extends.</returns>
        public static IdentityDapperOptions UseNpgsql(this IdentityDapperOptions options,
            string connectionString,
            string schema,
            string userTable,
            string roleTable,
            ICustomQueryRepository customQueryRepository)
        {
            options.ConnectionString = connectionString;
            options.Schema = schema ?? options.Schema;
            options.UserTable = userTable ?? options.UserTable;
            options.RoleTable = roleTable ?? options.RoleTable;

            options.Database = new Database.Npgsql(options, customQueryRepository);

            return options;
        }
    }
}
