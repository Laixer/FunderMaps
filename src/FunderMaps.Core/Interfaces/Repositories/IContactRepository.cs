using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
/// Operations for the contact repository.
/// </summary>
public interface IContactRepository : IAsyncRepository<Contact, string>
{
}
