using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.Data.Repositories
{
    internal class OrganizationUserRepository : IOrganizationUserRepository
    {
        /// <summary>
        ///     Data provider interface.
        /// </summary>
        public DbProvider DbProvider { get; }

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationUserRepository(DbProvider dbProvider) => DbProvider = dbProvider;

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
                    @role);
            ";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("organization_id", organizationId);
            cmd.AddParameterWithValue("role", role);

            await cmd.ExecuteNonQueryEnsureAffectedAsync().ConfigureAwait(false);
        }

        public async ValueTask IsUserInOrganization(Guid organizationId, Guid userId)
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
                SELECT  1
                FROM    application.organization_user
                WHERE   user_id = @user_id
                AND     organization_id = @organization_id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("user_id", userId);
            cmd.AddParameterWithValue("organization_id", organizationId);

            await cmd.ExecuteScalarEnsureRowAsync().ConfigureAwait(false);
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
