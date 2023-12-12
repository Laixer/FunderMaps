using FunderMaps.Core.Types.Products;

namespace FunderMaps.Core.Interfaces;

public interface IModelService
{
    Task<AnalysisProduct> GetAnalysisAsync(string id, Guid tenantId);
    Task<bool> GetRiskIndexAsync(string id, Guid tenantId);
    Task<StatisticsProduct> GetStatisticsAsync(string id);
}
