using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.MapBundle
{
    /// <summary>
    ///     Bundle service.
    /// </summary>
    public interface IBundleService
    {
        /// <summary>
        ///     Build a bundle.
        /// </summary>
        /// <param name="context">Bundle building context.</param>
        Task<Guid> BuildAsync(BundleBuildingContext context);

        /// <summary>
        ///     Build all bundles.
        /// </summary>
        /// <param name="context">Bundle building context.</param>
        Task<Guid> BuildAllAsync(BundleBuildingContext context);
    }
}
