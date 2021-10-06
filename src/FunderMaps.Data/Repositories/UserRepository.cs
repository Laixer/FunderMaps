using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

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
        public override async Task<Guid> AddAsync(User entity)
        {
            var sql = @"
                INSERT INTO application.user(
                    given_name,
                    last_name,
                    email,
                    avatar,
                    job_title,
                    phone_number,
                    role)
                VALUES (
                    @given_name,
                    @last_name,
                    @email,
                    @avatar,
                    NULLIF(trim(@job_title), ''),
                    REGEXP_REPLACE(@phone_number,'\D','','g'),
                    @role)
                RETURNING id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            MapToWriter(context, entity);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.user";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        // FUTURE: If user is in use it violates foreign key constraint, returning
        //         a ReferenceNotFoundException, which is invalid.
        /// <summary>
        ///     Delete <see cref="User"/>.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        public override async Task DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    application.user
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        public static void MapToWriter(DbContext context, User entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("given_name", entity.GivenName);
            context.AddParameterWithValue("last_name", entity.LastName);
            context.AddParameterWithValue("email", entity.Email);
            context.AddParameterWithValue("avatar", entity.Avatar);
            context.AddParameterWithValue("job_title", entity.JobTitle);
            context.AddParameterWithValue("phone_number", entity.PhoneNumber);
            context.AddParameterWithValue("role", entity.Role);
        }

        public static User MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new()
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
        /// <param name="id">Entity identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public override async Task<User> GetByIdAsync(Guid id)
        {
            if (TryGetEntity(id, out User entity))
            {
                return entity;
            }

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

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve <see cref="User"/> by email and password hash.
        /// </summary>
        /// <param name="email">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public async Task<User> GetByEmailAsync(string email)
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

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("email", email);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Get password hash.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Password hash as string.</returns>
        public async Task<string> GetPasswordHashAsync(Guid id)
        {
            var sql = @"
                SELECT  password_hash
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return reader.GetSafeString(0);
        }

        /// <summary>
        ///     Get access failed count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Failed access count.</returns>
        public async Task<int> GetAccessFailedCount(Guid id)
        {
            var sql = @"
                SELECT  access_failed_count
                FROM    application.user
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            return await context.ScalarAsync<int>();
        }

        /// <summary>
        ///     Retrieve all <see cref="User"/>.
        /// </summary>
        /// <returns>List of <see cref="User"/>.</returns>
        public override async IAsyncEnumerable<User> ListAllAsync(Navigation navigation)
        {
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

            sql = ConstructNavigation(sql, navigation, "u");

            await using var context = await DbContextFactory.CreateAsync(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Update <see cref="User"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(User entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            ResetCacheEntity(entity);

            var sql = @"
                UPDATE  application.user
                SET     given_name = @given_name,
                        last_name = @last_name,
                        avatar = @avatar,
                        job_title = NULLIF(trim(@job_title), ''),
                        phone_number = REGEXP_REPLACE(@phone_number,'\D','','g'),
                        role = @role
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Update user password.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="passwordHash">New password hash.</param>
        public async Task SetPasswordHashAsync(Guid id, string passwordHash)
        {
            var sql = @"
                UPDATE  application.user
                SET     password_hash = @password_hash
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("password_hash", passwordHash);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Increase signin failure count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        public async Task BumpAccessFailed(Guid id)
        {
            var sql = @"
                UPDATE  application.user
                SET     access_failed_count = access_failed_count + 1
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Reset signin failure count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        public async Task ResetAccessFailed(Guid id)
        {
            var sql = @"
                UPDATE  application.user
                SET     access_failed_count = 0
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Register a new user login.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        public async Task RegisterAccess(Guid id)
        {
            // FUTURE: Maybe call SP directly.
            var sql = @"
                SELECT application.log_access(@id)";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync(affectedGuard: false);
        }
    }
}
