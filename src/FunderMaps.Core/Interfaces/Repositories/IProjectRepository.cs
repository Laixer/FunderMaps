using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the project repository.
    /// </summary>
    public interface IProjectRepository : IAsyncRepository<Project, int>
    {
    }
}
