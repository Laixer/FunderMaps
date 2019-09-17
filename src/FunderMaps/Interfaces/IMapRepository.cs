using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Operations for the report repository.
    /// </summary>
    public interface IMapRepository
    {
        Task<IReadOnlyList<AddressPoint>> GetByOrganizationIdAsync(Guid orgId);
    }
}
