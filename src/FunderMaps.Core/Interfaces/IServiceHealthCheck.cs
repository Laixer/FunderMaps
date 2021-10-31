using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Health check service.
/// </summary>
public interface IServiceHealthCheck
{
    /// <summary>
    ///     Test if the service is alive.
    /// </summary>
    Task HealthCheck();
}
