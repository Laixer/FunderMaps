using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Organization user repository.
/// </summary>
public interface IOrganizationUserRepository
{
    /// <summary>
    ///     Add user to organization.
    /// </summary>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="userId">User identifier.</param>
    /// <param name="role">User role in organization.</param>
    Task AddAsync(Guid organizationId, Guid userId, OrganizationRole role);

    /// <summary>
    ///     Test if user belongs to an organization.
    /// </summary>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="userId">User identifier.</param>
    /// <returns><c>True</c> if user is member of organization, false otherwise.</returns>
    Task<bool> IsUserInOrganization(Guid organizationId, Guid userId);

    /// <summary>
    ///     Find all organizations by user identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>List of organization identifiers.</returns>
    IAsyncEnumerable<Guid> ListAllOrganizationIdByUserIdAsync(Guid userId);

    /// <summary>
    ///     Get user role within the organization.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="organizationId">Organization identifier.</param>
    /// <returns>Organization role.</returns>
    Task<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId, Guid organizationId);

    /// <summary>
    ///     List all organization members.
    /// </summary>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="navigation">Recordset nagivation.</param>
    /// <returns>List of user identifiers.</returns>
    IAsyncEnumerable<OrganizationUser> ListAllAsync(Guid organizationId, Navigation navigation);

    /// <summary>
    ///     List all organization members per role.
    /// </summary>
    /// <param name="organizationId">Organization identifier.</param>
    /// <param name="organizationRole">Organization roles.</param>
    /// <param name="navigation">Recordset nagivation.</param>
    /// <returns>List of user identifiers.</returns>
    IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] organizationRole, Navigation navigation);

    /// <summary>
    ///     Set user role within the organization.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="role">Organization role.</param>
    Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role);
}
