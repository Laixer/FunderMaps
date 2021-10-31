using FunderMaps.Core.Entities;
using System;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     Organization proposal repository.
/// </summary>
public interface IOrganizationProposalRepository : IAsyncRepository<OrganizationProposal, Guid>
{
}
