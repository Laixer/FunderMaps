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
        ValueTask<AnalysisProduct> GetByIdAsync(string id);

        ValueTask<AnalysisProduct> GetByIdInFenceAsync(Guid userId, string id);

        ValueTask<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalDataSource);

        /// <summary>
        ///     Retrieve <see cref="AnalysisProduct"/> by search query.
        /// </summary>
        IAsyncEnumerable<AnalysisProduct> GetBySearchQueryAsync(Guid userId, string query, INavigation navigation);

        ValueTask<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation);
    }
}
