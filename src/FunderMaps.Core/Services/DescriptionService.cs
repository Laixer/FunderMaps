using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Services
{
    /// TODO Implement.
    /// <summary>
    ///     Generates descriptions based on some <see cref="AnalysisProduct"/>.
    /// </summary>
    public sealed class DescriptionService : IDescriptionService
    {
        public string GenerateFullDescription(AnalysisProduct product) => "Full description";

        public string GenerateTerrainDescription(AnalysisProduct product) => "Terrain description";
    }
}
