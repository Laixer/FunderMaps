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
    /// <remarks>
    ///     Under no condition should the password_hash be
    ///     returned by this repository.
    /// </remarks>
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

            // TODO: normalized_email should be db trigger function
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            MapToWriter(cmd, entity);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

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

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
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
                GivenName = reader.SafeGetString(1),
                LastName = reader.SafeGetString(2),
                Email = reader.SafeGetString(3),
                Avatar = reader.SafeGetString(4),
                JobTitle = reader.SafeGetString(5),
                PhoneNumber = reader.SafeGetString(6),
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="User"/> by id and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask<User> GetByIdAndPasswordHashAsync(Guid id, string passwordHash)
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
                AND     password_hash = @password_hash
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            cmd.AddParameterWithValue("password_hash", passwordHash);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="User"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="User"/>.</returns>
        public async ValueTask<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash)
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
                AND     password_hash = @password_hash
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);
            cmd.AddParameterWithValue("password_hash", passwordHash);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
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

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
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

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
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

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);
            cmd.AddParameterWithValue("password_hash", passwordHash);

            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
