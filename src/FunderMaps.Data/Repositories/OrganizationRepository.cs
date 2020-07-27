using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    // TODO: Too much logic

    /// <summary>
    ///     Organization repository.
    /// </summary>
    internal class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        /// <summary>
        ///     Create new <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Organization"/>.</returns>
        public override async ValueTask<Guid> AddAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // TODO: normalized_email should be db trigger function
            var sql = @"
                INSERT INTO application.organization(
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

        public async ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            // TODO: Check id

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            // TODO: normalized_email should be db trigger function
            var sql = @"
	            SELECT application.create_organization(
                    @id,
                    @email,
                    @passwordHash)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", id);
            cmd.AddParameterWithValue("email", email);
            cmd.AddParameterWithValue("passwordHash", passwordHash);

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
                FROM    application.organization";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            var sql = @"
                DELETE
                FROM    application.organization
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }

        private static void MapToWriter(DbCommand cmd, Organization entity)
        {
            //cmd.AddParameterWithValue("given_name", entity.GivenName);
            //cmd.AddParameterWithValue("last_name", entity.LastName);
            //cmd.AddParameterWithValue("email", entity.Email);
            //cmd.AddParameterWithValue("avatar", entity.Avatar);
            //cmd.AddParameterWithValue("job_title", entity.JobTitle);
            //cmd.AddParameterWithValue("phone_number", entity.PhoneNumber);
            //cmd.AddParameterWithValue("role", entity.Role);
        }

        private static Organization MapFromReader(DbDataReader reader)
            => new Organization
            {
                //Id = reader.GetGuid(0),
                //GivenName = reader.SafeGetString(1),
                //LastName = reader.SafeGetString(2),
                //Email = reader.SafeGetString(3),
                //Avatar = reader.SafeGetString(4),
                //JobTitle = reader.SafeGetString(5),
                //PhoneNumber = reader.SafeGetString(6),
                //Role = reader.GetFieldValue<ApplicationRole>(7),
            };

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public override async ValueTask<Organization> GetByIdAsync(Guid id)
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
                FROM    application.organization
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
        ///     Retrieve <see cref="Organization"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public async ValueTask<Organization> GetByNameAsync(string name)
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
                FROM    application.organization
                WHERE   normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("name", name);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public async ValueTask<Organization> GetByEmailAsync(string email)
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
                FROM    application.organization
                WHERE   normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return MapFromReader(reader);
        }

        public async ValueTask<string> GetFenceAsync(Organization entity)
        {
            var sql = @"
                SELECT  ST_AsText(fence) AS fence
                FROM    application.organization
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return reader.SafeGetString(0);
        }

        /// <summary>
        ///     Retrieve all <see cref="Organization"/>.
        /// </summary>
        /// <returns>List of <see cref="Organization"/>.</returns>
        public override async IAsyncEnumerable<Organization> ListAllAsync(INavigation navigation)
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
                FROM    application.organization";

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
        ///     Update <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE  application.organization
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
    }
}
