using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Organization user repository.
    /// </summary>
    internal class OrganizationUserRepository : DbContextBase, IOrganizationUserRepository
    {
        public async ValueTask AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
        {
            var sql = @"
                INSERT INTO application.organization_user(
                    user_id,
                    organization_id,
                    role)
                VALUES (
                    @user_id,
                    @organization_id,
                    @role)";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user_id", userId);
            context.AddParameterWithValue("organization_id", organizationId);
            context.AddParameterWithValue("role", role);

            await context.NonQueryAsync();
        }

        /// <summary>
        ///     Retrieve all users by organization.
        /// </summary>
        /// <returns>List of user identifiers.</returns>
        public async IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId, INavigation navigation)
        {
            var sql = @"
                SELECT  user_id
                FROM    application.organization_user
                WHERE   organization_id = @organization_id";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("organization_id", organizationId);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return reader.GetGuid(0);
            }
        }

        public async IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] role, INavigation navigation)
        {
            var sql = @"
                SELECT  user_id
                FROM    application.organization_user
                WHERE   organization_id = @organization_id
                AND     role = ANY(@role)";

            ConstructNavigation(ref sql, navigation);

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("organization_id", organizationId);
            context.AddParameterWithValue("role", role);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return reader.GetGuid(0);
            }
        }

        public async ValueTask<bool> IsUserInOrganization(Guid organizationId, Guid userId)
        {
            // FUTURE: database function
            var sql = @"
                SELECT EXISTS (
                    SELECT  1
                    FROM    application.organization_user
                    WHERE   user_id = @user_id
                    AND     organization_id = @organization_id
                    LIMIT   1
                )";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user_id", userId);
            context.AddParameterWithValue("organization_id", organizationId);

            return await context.ScalarAsync<bool>();
        }

        public async ValueTask<Guid> GetOrganizationByUserIdAsync(Guid userId)
        {
            var sql = @"
                SELECT  organization_id
                FROM    application.organization_user
                WHERE   user_id = @user_id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user_id", userId);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        public async ValueTask<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId)
        {
            var sql = @"
                SELECT  role
                FROM    application.organization_user
                WHERE   user_id = @user_id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user_id", userId);

            await using var reader = await context.ReaderAsync();

            return reader.GetFieldValue<OrganizationRole>(0);
        }

        public async Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role)
        {
            var sql = @"
                UPDATE  application.organization_user
                SET     role = @role
                WHERE   user_id = @user_id";

            await using var context = await DbContextFactory(sql);

            context.AddParameterWithValue("user_id", userId);
            context.AddParameterWithValue("role", role);

            await context.NonQueryAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
