using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Contract for generating descriptions.
    /// </summary>
    public interface IDescriptionService
    {
        string GenerateFullDescription(AnalysisProduct product);

        string GenerateTerrainDescription(AnalysisProduct product);
    }
}
