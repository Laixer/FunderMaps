using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data.Abstractions;
using FunderMaps.Data.Extensions;

namespace FunderMaps.Data.Repositories;

/// <summary>
///     Organization user repository.
/// </summary>
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("user_id", userId);
        context.AddParameterWithValue("organization_id", organizationId);
        context.AddParameterWithValue("role", role);

        await context.NonQueryAsync();
    }

    /// <summary>
    ///     Retrieve all users by organization.
    /// </summary>
    /// <returns>List of user identifiers.</returns>
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("organization_id", organizationId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return new()
            {
                Id = reader.GetGuid(0),
                GivenName = reader.GetSafeString(1),
                LastName = reader.GetSafeString(2),
                Email = reader.GetString(3),
                JobTitle = reader.GetSafeString(4),
                PhoneNumber = reader.GetSafeString(5),
                Role = reader.GetFieldValue<ApplicationRole>(6),
                OrganizationRole = reader.GetFieldValue<OrganizationRole>(7),
            };
        }
    }

    public async IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] role, Navigation navigation)
    {
        var sql = @"
            SELECT  user_id
            FROM    application.organization_user
            WHERE   organization_id = @organization_id
            AND     role = ANY(@role)";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("organization_id", organizationId);
        context.AddParameterWithValue("role", role);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return reader.GetGuid(0);
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

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("user_id", userId);
        context.AddParameterWithValue("organization_id", organizationId);

        return await context.ScalarAsync<bool>();
    }

    public async IAsyncEnumerable<Guid> ListAllOrganizationIdByUserIdAsync(Guid userId)
    {
        var sql = @"
            SELECT  organization_id
            FROM    application.organization_user
            WHERE   user_id = @user_id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("user_id", userId);

        await foreach (var reader in context.EnumerableReaderAsync())
        {
            yield return reader.GetGuid(0);
        }
    }

    public async Task<OrganizationRole?> GetOrganizationRoleByUserIdAsync(Guid userId, Guid organizationId)
    {
        var sql = @"
            SELECT  role
            FROM    application.organization_user
            WHERE   user_id = @user_id
            AND     organization_id = @organization_id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("user_id", userId);
        context.AddParameterWithValue("organization_id", organizationId);

        await using var reader = await context.ReaderAsync();

        return reader.GetSafeStructValue<OrganizationRole>(0);
    }

    // TODO: Also request the organization for which this role must be set.
    public async Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role)
    {
        var sql = @"
            UPDATE  application.organization_user
            SET     role = @role
            WHERE   user_id = @user_id";

        await using var context = await DbContextFactory.CreateAsync(sql);

        context.AddParameterWithValue("user_id", userId);
        context.AddParameterWithValue("role", role);

        await context.NonQueryAsync();
    }
}
