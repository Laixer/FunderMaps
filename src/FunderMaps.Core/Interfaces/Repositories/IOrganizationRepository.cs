﻿using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Organization repository.
/// </summary>
public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
{
    /// <summary>
    ///     Count number of organizations.
    /// </summary>
    Task<long> CountAsync();

    /// <summary>
    ///     Retrieve all organizations.
    /// </summary>
    IAsyncEnumerable<Organization> ListAllAsync(Navigation navigation);

    /// <summary>
    ///     Delete organization by identifier.
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    ///     Retrieve organization by identifier.
    /// </summary>
    Task UpdateAsync(Organization entity);
}
