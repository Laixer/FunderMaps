using System.Threading.Tasks;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Bundle service.
    /// </summary>
    public interface IBundleService
    {
        /// <summary>
        ///     Build outdated bundles.
        /// </summary>
        Task BuildAsync();
    }
}
