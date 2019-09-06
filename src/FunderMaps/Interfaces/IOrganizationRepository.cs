using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using System;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Organization repository.
    /// </summary>
    public interface IOrganizationRepository : IAsyncRepository<Organization, Guid> { }
}
