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
            #region IUserStore
            CreateAsync = $@"
                INSERT INTO {_options.Schema}.{_options.UserTable} (
                username, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_end, lockout_enabled, access_failed_count, attestation_principal_id)
                VALUES(@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, 0, 0)";

            DeleteAsync = $"DELETE FROM {_options.Schema}.{_options.UserTable} WHERE Id=@Id";

            FindByIdAsync = $"SELECT * FROM {_options.Schema}.{_options.UserTable} WHERE Id=@Id LIMIT 1";

            FindByNameAsync = $"SELECT * FROM {_options.Schema}.{_options.UserTable} WHERE normalized_username=@NormalizedUserName LIMIT 1";

            GetNormalizedUserNameAsync = $"SELECT normalized_username FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            GetUserIdAsync = $"SELECT id FROM {_options.Schema}.{_options.UserTable} WHERE normalized_username=@NormalizedUserName OR normalized_email=@NormalizedEmail LIMIT 1";

            GetUserNameAsync = $"SELECT username FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            SetNormalizedUserNameAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET normalized_username=@NormalizedUserName WHERE id=@Id";

            SetUserNameAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET username=@UserName WHERE id=@Id";

            UpdateAsync = $@"
                    UPDATE {_options.Schema}.{_options.UserTable}
                    SET username=@UserName, normalized_username=@NormalizedUserName, email=@Email, normalized_email=@NormalizedEmail, email_confirmed=@EmailConfirmed, password_hash=@PasswordHash, security_stamp=@SecurityStamp, concurrency_stamp=@ConcurrencyStamp, phone_number=@PhoneNumber, phone_number_confirmed=@PhoneNumberConfirmed, two_factor_enabled=@TwoFactorEnabled, lockout_end=@LockoutEnd, lockout_enabled=@LockoutEnabled, access_failed_count=@AccessFailedCount
                    WHERE id=@Id";
            #endregion

            #region IUserEmailStore
            FindByEmailAsync = $"SELECT * FROM {_options.Schema}.{_options.UserTable} WHERE normalized_email=@NormalizedEmail LIMIT 1";

            GetEmailAsync = $"SELECT email FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            GetEmailConfirmedAsync = $"SELECT email_confirmed FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            GetNormalizedEmailAsync = $"SELECT normalized_email FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            SetEmailAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET email=@Email WHERE id=@Id";

            SetEmailConfirmedAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET email_confirmed=@EmailConfirmed WHERE id=@Id";

            SetNormalizedEmailAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET normalized_email=@NormalizedEmail WHERE id=@Id";
            #endregion



            #region IUserPasswordStore
            GetPasswordHashAsync = $"SELECT password_hash FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            SetPasswordHashAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET password_hash=@PasswordHash WHERE id=@Id";
            #endregion

            #region IUserSecurityStampStore
            GetSecurityStampAsync = $"SELECT security_stamp FROM {_options.Schema}.{_options.UserTable} WHERE id=@Id LIMIT 1";

            SetSecurityStampAsync = $"UPDATE {_options.Schema}.{_options.UserTable} SET security_stamp=@SecurityStamp WHERE id=@Id";
            #endregion
        }

        /// <summary>
        /// Get the database connection.
        /// </summary>
        /// <returns>Instance of <see cref="NpgsqlConnection"/>.</returns>
        public override IDbConnection GetDbConnection() => new NpgsqlConnection(_options.ConnectionString);
    }
}
