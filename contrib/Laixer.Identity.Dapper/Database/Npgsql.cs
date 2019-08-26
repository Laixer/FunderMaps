using Npgsql;
using System.Data;

namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Driver for PostgreSQL driver.
    /// </summary>
    internal class Npgsql : DatabaseQeuryBase
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

            PrepQueries();
        }

        private void PrepQueries()
        {
            CreateAsync = $@"
                INSERT INTO {_options.Schema}.{_options.UserTable} (
                username, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_end, lockout_enabled, access_failed_count, attestation_principal_id)
                VALUES(@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, 0, 0)";

            FindByEmailAsync = $"SELECT * FROM {_options.Schema}.{_options.UserTable} WHERE normalized_email=@NormalizedEmail LIMIT 1";

            GetPasswordHashAsync = $"SELECT password_hash FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";
        }

        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="NpgsqlConnection"/>.</returns>
        public override IDbConnection GetDbConnection() => new NpgsqlConnection(_options.ConnectionString);
    }
}
