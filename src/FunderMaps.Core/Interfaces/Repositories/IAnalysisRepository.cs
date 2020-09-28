using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IAnalysisRepository
    {
        Task<AnalysisProduct> GetByIdAsync(string id, CancellationToken token = default);

        Task<AnalysisProduct> GetByIdInFenceAsync(Guid userId, string id, CancellationToken token = default);

        Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalDataSource, CancellationToken token = default);

        Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation, CancellationToken token = default);

        Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation, CancellationToken token = default);
    }
}
