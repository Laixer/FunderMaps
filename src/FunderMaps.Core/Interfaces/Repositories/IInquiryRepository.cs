﻿using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Operations for the inquiry repository.
/// </summary>
public interface IInquiryRepository
{
    Task<int> AddAsync(Inquiry entity);

    Task<long> CountAsync(Guid tenantId);

    Task DeleteAsync(int id, Guid tenantId);

    Task UpdateAsync(Inquiry entity);

    Task<Inquiry> GetByIdAsync(int id, Guid tenantId);

    IAsyncEnumerable<Inquiry> ListAllAsync(Navigation navigation, Guid tenantId);

    IAsyncEnumerable<Inquiry> ListAllByBuildingIdAsync(Navigation navigation, Guid tenantId, string id);

    /// <summary>
    ///     Set <see cref="InquiryFull"/> audit status.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="entity">Entity object.</param>
    Task SetAuditStatusAsync(int id, Inquiry entity, Guid tenantId);
}
