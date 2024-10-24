﻿using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the inquiry sample repository.
/// </summary>
public interface IInquirySampleRepository
{
    Task<int> AddAsync(InquirySample entity);

    // TODO: Add tenantId
    IAsyncEnumerable<InquirySample> ListAllByBuildingIdAsync(string id);

    /// <summary>
    ///     Retrieve number of entities and filter on report.
    /// </summary>
    /// <returns>Number of entities.</returns>
    Task<long> CountAsync(int report, Guid tenantId);

    Task DeleteAsync(int id, Guid tenantId);

    Task<InquirySample> GetByIdAsync(int id, Guid tenantId);

    /// <summary>
    ///     Retrieve all entities and filter on report.
    /// </summary>
    /// <returns>List of entities.</returns>
    IAsyncEnumerable<InquirySample> ListAllAsync(int report, Navigation navigation, Guid tenantId);

    Task UpdateAsync(InquirySample entity, Guid tenantId);
}
