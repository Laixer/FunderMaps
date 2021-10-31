using System.Threading.Tasks;
namespace FunderMaps.Core.MapBundle;

/// <summary>
///     Bundle service.
/// </summary>
public interface IBundleService
{
    /// <summary>
    ///     Build bundles.
    /// </summary>
    Task BuildAsync();
}
