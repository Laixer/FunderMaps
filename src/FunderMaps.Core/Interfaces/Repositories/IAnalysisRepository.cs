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

        Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId);

        Task<AnalysisProduct> GetByAddressExternalIdAsync(Guid userId, string externalId);

        /// <summary>
        ///     Retrieve <see cref="AnalysisProduct"/> by search query.
        /// </summary>
        IAsyncEnumerable<AnalysisProduct> GetBySearchQueryAsync(Guid userId, string query, INavigation navigation);
    }
}
