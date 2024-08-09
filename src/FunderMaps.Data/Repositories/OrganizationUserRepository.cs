using Dapper;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Abstractions;

namespace FunderMaps.Data.Repositories;

internal class OrganizationUserRepository : DbServiceBase, IOrganizationUserRepository
{
    public async Task AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
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

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { user_id = userId, organization_id = organizationId, role });
    }

    public async IAsyncEnumerable<OrganizationUser> ListAllAsync(Guid organizationId, Navigation navigation)
    {
        var sql = @"
            SELECT
                    u.id,
                    u.given_name,
                    u.last_name,
                    u.email,
                    u.job_title,
                    u.phone_number,
                    u.role,
                    ou.role AS organization_role
            FROM   application.user u
            JOIN   application.organization_user ou ON ou.user_id = u.id
            WHERE  ou.organization_id = @organization_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<OrganizationUser>(sql, navigation))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] role, Navigation navigation)
    {
        var sql = @"
            SELECT  user_id
            FROM    application.organization_user
            WHERE   organization_id = @organization_id
            AND     role = ANY(@role)";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Guid>(sql, new { organization_id = organizationId, role }))
        {
            yield return item;
        }
    }

    public async Task<bool> IsUserInOrganization(Guid organizationId, Guid userId)
    {
        var sql = @"
            SELECT EXISTS (
                SELECT  1
                FROM    application.organization_user
                WHERE   user_id = @user_id
                AND     organization_id = @organization_id
                LIMIT   1
            )";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.ExecuteScalarAsync<bool>(sql, new { user_id = userId, organization_id = organizationId });
    }

    public async IAsyncEnumerable<Guid> ListAllOrganizationIdByUserIdAsync(Guid userId)
    {
        var sql = @"
            SELECT  organization_id
            FROM    application.organization_user
            WHERE   user_id = @user_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await foreach (var item in connection.QueryUnbufferedAsync<Guid>(sql, new { user_id = userId }))
        {
            yield return item;
        }
    }

    public async Task<OrganizationRole?> GetOrganizationRoleByUserIdAsync(Guid userId, Guid organizationId)
    {
        var sql = @"
            SELECT  role
            FROM    application.organization_user
            WHERE   user_id = @user_id
            AND     organization_id = @organization_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        return await connection.QuerySingleOrDefaultAsync<OrganizationRole?>(sql, new { user_id = userId, organization_id = organizationId });
    }

    // TODO: Also request the organization for which this role must be set.
    public async Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role)
    {
        var sql = @"
            UPDATE  application.organization_user
            SET     role = @role
            WHERE   user_id = @user_id";

        await using var connection = DbContextFactory.DbProvider.ConnectionScope();

        await connection.ExecuteAsync(sql, new { user_id = userId, role });
    }
}
