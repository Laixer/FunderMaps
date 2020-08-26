using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Services
{
    // FUTURE Implement.
    /// <summary>
    ///     Generates descriptions based on some <see cref="AnalysisProduct"/>.
    /// </summary>
    public sealed class DescriptionService : IDescriptionService
    {
        /// <summary>
        ///     Generates a full description for an <see cref="AnalysisProduct"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <returns>Full description.</returns>
        public string GenerateFullDescription(AnalysisProduct product) => "Full description";

        /// <summary>
        ///     Generates a terrain description for an <see cref="AnalysisProduct"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <returns>Terrain description.</returns>
        public string GenerateTerrainDescription(AnalysisProduct product) => "Terrain description";
    }
}
