using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    internal class OrganizationUserRepository : DataBase, IOrganizationUserRepository
    {
        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationUserRepository(DbProvider dbProvider)
            : base(dbProvider)
        {
        }

        public async ValueTask AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
        {
            if (organizationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(organizationId));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var sql = @"
                INSERT INTO application.organization_user(
                    user_id,
                    organization_id,
                    role)
                VALUES (
                    @user_id,
                    @organization_id,
                    @role)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("organization_id", organizationId);
            cmd.AddParameterWithValue("role", role);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        /// <summary>
        ///     Retrieve all users by organization.
        /// </summary>
        /// <returns>List of user identifiers.</returns>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public async IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId, INavigation navigation)
        {
            var sql = @"
                SELECT  user_id
                FROM    application.organization_user
                WHERE   organization_id = @organization_id";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("organization_id", organizationId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            while (await reader.ReadAsync())
            {
                yield return reader.GetGuid(0);
            }
        }

        /// <summary>
        ///     Retrieve all <see cref="Incident"/>.
        /// </summary>
        /// <returns>List of <see cref="Incident"/>.</returns>
        /// <exception cref="NullResultException"> is thrown if statement had no affect.</exception>
        public async IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole role, INavigation navigation)
        {
            var sql = @"
                SELECT  user_id
                FROM    application.organization_user
                WHERE   organization_id = @organization_id
                AND     role = @role";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("organization_id", organizationId);
            cmd.AddParameterWithValue("role", role);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            while (await reader.ReadAsync())
            {
                yield return reader.GetGuid(0);
            }
        }

        public async ValueTask<bool> IsUserInOrganization(Guid organizationId, Guid userId)
        {
            if (organizationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(organizationId));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            // FUTURE: database function
            var sql = @"
                SELECT EXISTS (
                    SELECT  1
                    FROM    application.organization_user
                    WHERE   user_id = @user_id
                    AND     organization_id = @organization_id
                    LIMIT   1
                )";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("organization_id", organizationId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();
            return reader.GetBoolean(0);
        }

        public async ValueTask<Guid> GetOrganizationByUserIdAsync(Guid userId)
        {
            var sql = @"
                SELECT  organization_id
                FROM    application.organization_user
                WHERE   user_id = @user_id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();
            return reader.GetGuid(0);
        }

        public async ValueTask<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId)
        {
            var sql = @"
                SELECT  role
                FROM    application.organization_user
                WHERE   user_id = @user_id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();
            return reader.GetFieldValue<OrganizationRole>(0);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
