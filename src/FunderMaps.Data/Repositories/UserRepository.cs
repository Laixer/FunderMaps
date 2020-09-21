using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     User repository.
    /// </summary>
    internal class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public UserRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Create new <see cref="User"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="User"/>.</returns>
        public override async ValueTask<Guid> AddAsync(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // FUTURE: normalized_email should be db trigger function
            var sql = @"
                INSERT INTO application.user(
                    given_name,
                    last_name,
                    email,
                    normalized_email,
                    avatar,
                    job_title,
                    phone_number,
                    role)
                VALUES (
                    @given_name,
                    @last_name,
                    @email,
                    application.normalize(@email),
                    @avatar,
                    @job_title,
                    @phone_number,
                    @role)
                RETURNING id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.user";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="User"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            var sql = @"
                DELETE
                FROM    application.user
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        private static void MapToWriter(DbCommand cmd, User entity)
        {
            cmd.AddParameterWithValue("given_name", entity.GivenName);
            cmd.AddParameterWithValue("last_name", entity.LastName);
            cmd.AddParameterWithValue("email", entity.Email);
            cmd.AddParameterWithValue("avatar", entity.Avatar);
            cmd.AddParameterWithValue("job_title", entity.JobTitle);
            cmd.AddParameterWithValue("phone_number", entity.PhoneNumber);
            cmd.AddParameterWithValue("role", entity.Role);
        }

        private static User MapFromReader(DbDataReader reader)
            => new User
            {
                Id = reader.GetGuid(0),
                GivenName = reader.GetSafeString(1),
                LastName = reader.GetSafeString(2),
                Email = reader.GetSafeString(3),
                Avatar = reader.GetSafeString(4),
                JobTitle = reader.GetSafeString(5),
                PhoneNumber = reader.GetSafeString(6),
                Role = reader.GetFieldValue<ApplicationRole>(7),
            };

        /// <summary>
        ///     Retrieve <see cref="User"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public override async ValueTask<User> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  id,
                        given_name,
                        last_name,
                        email,
                        avatar,
                        job_title,
                        phone_number,
                        role
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="User"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask<User> GetByEmailAsync(string email)
        {
            var sql = @"
                SELECT  id,
                        given_name,
                        last_name,
                        email,
                        avatar,
                        job_title,
                        phone_number,
                        role
                FROM    application.user
                WHERE   normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        public async ValueTask<uint> GetAccessFailedCountAsync(User entity)
        {
            var sql = @"
                SELECT  access_failed_count
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetUInt(0);
        }

        public async ValueTask<uint?> GetLoginCountAsync(User entity)
        {
            var sql = @"
                SELECT  login_count
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetUInt(0);
        }

        public async ValueTask<DateTime?> GetLastLoginAsync(User entity)
        {
            var sql = @"
                SELECT  last_login
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetSafeDateTime(0);
        }

        public async ValueTask<string> GetPasswordHashAsync(User entity)
        {
            var sql = @"
                SELECT  password_hash
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetSafeString(0);
        }

        public async ValueTask<bool> IsLockedOutAsync(User entity)
        {
            var sql = @"
                SELECT EXISTS (
                    SELECT  *
                    FROM    application.user
                    WHERE   id = @id
                    AND     lockout_end > NOW()
                    LIMIT   1
                ) AS is_locked";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            return (bool)await cmd.ExecuteScalarEnsureRowAsync();
        }

        /// <summary>
        ///     Retrieve all <see cref="User"/>.
        /// </summary>
        /// <returns>List of <see cref="User"/>.</returns>
        public override async IAsyncEnumerable<User> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        given_name,
                        last_name,
                        email,
                        avatar,
                        job_title,
                        phone_number,
                        role
                FROM    application.user";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="User"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE  application.user
                SET     given_name = @given_name,
                        last_name = @last_name,
                        email = @email,
                        normalized_email = application.normalize(@email),
                        avatar = @avatar,
                        job_title = @job_title,
                        phone_number = @phone_number,
                        role = @role
                WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        /// <summary>
        ///     Update <see cref="User"/> password hash.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public async ValueTask SetPasswordHashAsync(User entity, string passwordHash)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE  application.user
                SET     password_hash = @password_hash
                WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);
            cmd.AddParameterWithValue("password_hash", passwordHash);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        /// <summary>
        ///     Update <see cref="User"/> access failed count.
        /// </summary>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask BumpAccessFailed(User entity)
        {
            var sql = @"
                UPDATE  application.user
                SET     access_failed_count = access_failed_count + 1
                WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        /// <summary>
        ///     Update <see cref="User"/> access failed count.
        /// </summary>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask ResetAccessFailed(User entity)
        {
            var sql = @"
                UPDATE  application.user
                SET     access_failed_count = 0
                WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        /// <summary>
        ///     Update <see cref="User"/> access.
        /// </summary>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask RegisterAccess(User entity)
        {
            // FUTURE: db func
            // FUTURE: db trigger to update last_login
            var sql = @"
                UPDATE  application.user
                SET     login_count = login_count + 1,
                        last_login = CURRENT_TIMESTAMP
                WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
