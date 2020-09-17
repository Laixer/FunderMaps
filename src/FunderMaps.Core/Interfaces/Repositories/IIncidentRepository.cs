using FunderMaps.Core.Entities;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    /// Operations for the incident repository.
    /// </summary>
    public interface IIncidentRepository : IAsyncRepository<Incident, string>
    {
    }
}
