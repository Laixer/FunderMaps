using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    /// Operations for the report repository.
    /// </summary>
    public interface IMapRepository
    {
        /// <summary>
        /// Retrieve sample addresses by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressPoint>> GetByOrganizationIdAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation recovery and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressPoint>> GetFounationRecoveryByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation type 'wood' and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFounationTypeWoodByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation type 'concrete' and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFounationTypeConcreteByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation type 'no pile' and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFounationTypeNoPileByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation type 'wood charger' and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFounationTypeWoodChargerByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation type 'other' and by organization.
        /// </summary>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFounationTypeOtherByOrganizationAsync(Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by enforcement term range and by organization.
        /// </summary>
        /// <param name="rangeStart">Beginning of enforcemen term range.</param>
        /// <param name="rangeEnd">End of enforcemen term range.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByEnforcementTermByOrganizationAsync(int rangeStart, int rangeEnd, Guid orgId);

        /// <summary>
        /// Retrieve sample addresses by foundation quality and by organization.
        /// </summary>
        /// <param name="foundationQuality">Foundation quality selector.</param>
        /// <param name="orgId">Organization identifier.</param>
        /// <returns>List of <see cref="AddressPoint"/>.</returns>
        Task<IReadOnlyList<AddressGeoJson>> GetByFoundationQualityByOrganizationAsync(FoundationQuality foundationQuality, Guid orgId);

        Task<IReadOnlyList<AddressGeoJson>> GetByFoundationSubsidenceByOrganizationAsync(double rangeStart, double rangeEnd, Guid orgId);
    }
}
