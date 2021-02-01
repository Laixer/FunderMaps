using FunderMaps.Core.Entities;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Service to the incidents.
    /// </summary>
    public interface IIncidentService
    {
        /// <summary>
        ///     Register a new incident.
        /// </summary>
        /// <param name="incident">Incident to process.</param>
        /// <param name="meta">Optional metadata.</param>
        Task<Incident> AddAsync(Incident incident, object meta = null);
    }
}