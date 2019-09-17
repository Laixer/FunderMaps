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
        Task<IReadOnlyList<AddressPoint>> GetFounationRecoveryByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFounationTypeWoodByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFounationTypeConcreteByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFounationTypeNoPileByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFounationTypeWoodChargerByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFounationTypeOtherByOrganizationAsync(Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByEnforcementTermByOrganizationAsync(int rangeStart, int rangeEnd, Guid orgId);
        Task<IReadOnlyList<AddressPoint>> GetByFoundationQualityByOrganizationAsync(FoundationQuality foundationQuality, Guid orgId);
    }
}
