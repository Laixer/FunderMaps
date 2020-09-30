using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
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

            await using var context = await DbContextFactory(sql);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.user";

            await using var context = await DbContextFactory(sql);

            return await context.ScalarAsync<ulong>();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        public static void MapToWriter(DbContext context, User entity)
        {
            context.AddParameterWithValue("given_name", entity.GivenName);
            context.AddParameterWithValue("last_name", entity.LastName);
            context.AddParameterWithValue("email", entity.Email);
            context.AddParameterWithValue("avatar", entity.Avatar);
            context.AddParameterWithValue("job_title", entity.JobTitle);
            context.AddParameterWithValue("phone_number", entity.PhoneNumber);
            context.AddParameterWithValue("role", entity.Role);
        }

        public static User MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new User
            {
                Id = reader.GetGuid(offset + 0),
                GivenName = reader.GetSafeString(offset + 1),
                LastName = reader.GetSafeString(offset + 2),
                Email = reader.GetSafeString(offset + 3),
                Avatar = reader.GetSafeString(offset + 4),
                JobTitle = reader.GetSafeString(offset + 5),
                PhoneNumber = reader.GetSafeString(offset + 6),
                Role = reader.GetFieldValue<ApplicationRole>(offset + 7),
            };

        /// <summary>
        ///     Retrieve <see cref="User"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public override async ValueTask<User> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  -- User
                        u.id,
                        u.given_name,
                        u.last_name,
                        u.email,
                        u.avatar,
                        u.job_title,
                        u.phone_number,
                        u.role
                FROM    application.user AS u
                WHERE   u.id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

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
                SELECT  -- User
                        u.id,
                        u.given_name,
                        u.last_name,
                        u.email,
                        u.avatar,
                        u.job_title,
                        u.phone_number,
                        u.role
                FROM    application.user AS u
                WHERE   u.normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("email", email);

            await using var reader = await context.ReaderAsync();

            return MapFromReader(reader);
        }

        public async ValueTask<uint> GetAccessFailedCountAsync(User entity)
        {
            var sql = @"
                SELECT  access_failed_count
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            return await context.ScalarAsync<uint>();
        }

        // FUTURE: Why nullable return?
        public async ValueTask<uint?> GetLoginCountAsync(User entity)
        {
            var sql = @"
                SELECT  login_count
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            return await context.ScalarAsync<uint>();
        }

        public async ValueTask<DateTime?> GetLastLoginAsync(User entity)
        {
            var sql = @"
                SELECT  last_login
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            await using var reader = await context.ReaderAsync();

            return reader.GetSafeDateTime(0);
        }

        public async ValueTask<string> GetPasswordHashAsync(User entity)
        {
            var sql = @"
                SELECT  password_hash
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            await using var reader = await context.ReaderAsync();

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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            return await context.ScalarAsync<bool>();
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
                SELECT  u.id,
                        u.given_name,
                        u.last_name,
                        u.email,
                        u.avatar,
                        u.job_title,
                        u.phone_number,
                        u.role
                FROM    application.user AS u";

            ConstructNavigation(ref sql, navigation, "u");

            await using var context = await DbContextFactory(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);
            context.AddParameterWithValue("password_hash", passwordHash);

            await context.NonQueryAsync();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            await context.NonQueryAsync();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            await context.NonQueryAsync();
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

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("id", entity.Id);

            await context.NonQueryAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
