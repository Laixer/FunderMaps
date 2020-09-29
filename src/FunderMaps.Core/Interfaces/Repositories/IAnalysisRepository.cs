using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Operations for the analysis repository.
    /// </summary>
    public interface IAnalysisRepository
    {
        Task<AnalysisProduct> GetByIdAsync(string id);

        Task<AnalysisProduct> GetByIdInFenceAsync(Guid userId, string id);

        Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalDataSource);

        Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation);

        Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation);
    }
}
