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
                username, normalized_username, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_end, lockout_enabled, access_failed_count)
                VALUES(@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, 0) RETURNING id";

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
        }
    }
}
