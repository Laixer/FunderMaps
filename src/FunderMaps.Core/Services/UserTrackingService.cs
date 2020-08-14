using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Service for tracking user behaviour with regards to products.
    /// </summary>
    public sealed class UserTrackingService : IUserTrackingService
    {
        public Task ProcessAnalysisRequest(Guid userId, AnalysisProductType productType) => throw new NotImplementedException();
        public Task ProcessStatisticsRequestAsync(Guid userId, StatisticsProductType productType) => throw new NotImplementedException();
    }
}
