namespace Laixer.Identity.Dapper.Database
{
    /// <summary>
    /// Driver for PostgreSQL driver.
    /// </summary>
    internal partial class Npgsql
    {
        private void PrepareQueries()
        {
            #region IUserStore
            QueryRepository.CreateAsync = $@"
                INSERT INTO {_options.Schema}.{_options.UserTable} (
                username, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_end, lockout_enabled, access_failed_count, attestation_principal_id)
                VALUES(@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, 0, 0)";

            QueryRepository.DeleteAsync = $@"
                DELETE FROM {_options.Schema}.{_options.UserTable} WHERE
                Id=@Id";

            QueryRepository.FindByIdAsync = $@"
                SELECT *
                FROM {_options.Schema}.{_options.UserTable}
                WHERE Id=@Id
                LIMIT 1";

            QueryRepository.FindByNameAsync = $@"
                SELECT *
                FROM {_options.Schema}.{_options.UserTable}
                WHERE normalized_username=@NormalizedUserName
                LIMIT 1";

            QueryRepository.GetNormalizedUserNameAsync = $@"
                SELECT normalized_username
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.GetUserIdAsync = $@"
                SELECT id
                FROM {_options.Schema}.{_options.UserTable}
                WHERE normalized_username=@NormalizedUserName
                    OR normalized_email=@NormalizedEmail
                LIMIT 1";

            QueryRepository.GetUserNameAsync = $@"
                SELECT username
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.SetNormalizedUserNameAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET normalized_username=@NormalizedUserName
                WHERE id=@Id";

            QueryRepository.SetUserNameAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET username=@UserName
                WHERE id=@Id";

            QueryRepository.UpdateAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET username=@UserName, normalized_username=@NormalizedUserName, email=@Email, normalized_email=@NormalizedEmail, email_confirmed=@EmailConfirmed, password_hash=@PasswordHash, security_stamp=@SecurityStamp, concurrency_stamp=@ConcurrencyStamp, phone_number=@PhoneNumber, phone_number_confirmed=@PhoneNumberConfirmed, two_factor_enabled=@TwoFactorEnabled, lockout_end=@LockoutEnd, lockout_enabled=@LockoutEnabled, access_failed_count=@AccessFailedCount
                WHERE id=@Id";
            #endregion

            #region IUserEmailStore
            QueryRepository.FindByEmailAsync = $@"
                SELECT *
                FROM {_options.Schema}.{_options.UserTable}
                WHERE normalized_email=@NormalizedEmail
                LIMIT 1";

            QueryRepository.GetEmailAsync = $@"
                SELECT email
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.GetEmailConfirmedAsync = $@"
                SELECT email_confirmed
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.GetNormalizedEmailAsync = $@"
                SELECT normalized_email
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.SetEmailAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET email=@Email
                WHERE id=@Id";

            QueryRepository.SetEmailConfirmedAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET email_confirmed=@EmailConfirmed
                WHERE id=@Id";

            QueryRepository.SetNormalizedEmailAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET normalized_email=@NormalizedEmail
                WHERE id=@Id";
            #endregion

            #region IUserRoleStore
            // TODO:
            QueryRepository.AddToRoleAsync = $@"";

            QueryRepository.GetRolesAsync = $@"
                SELECT {_options.Schema}.role.name
                FROM {_options.Schema}.user_role
                JOIN {_options.Schema}.role ON {_options.Schema}.role.id = {_options.Schema}.user_role.role_id
                WHERE user_id=@Id";

            // TODO:
            QueryRepository.GetUsersInRoleAsync = $@"";

            // TODO:
            QueryRepository.RemoveFromRoleAsync = $@"";
            #endregion

            #region IUserPasswordStore
            QueryRepository.GetPasswordHashAsync = $@"
                SELECT password_hash
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.SetPasswordHashAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET password_hash=@PasswordHash
                WHERE id=@Id";
            #endregion

            #region IUserSecurityStampStore
            QueryRepository.GetSecurityStampAsync = $@"
                SELECT security_stamp
                FROM {_options.Schema}.{_options.UserTable}
                WHERE id=@Id
                LIMIT 1";

            QueryRepository.SetSecurityStampAsync = $@"
                UPDATE {_options.Schema}.{_options.UserTable}
                SET security_stamp=@SecurityStamp
                WHERE id=@Id";
            #endregion
        }
    }
}
