using FunderMaps.Webservice.InputModels;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Abstractions.Services
{
    /// <summary>
    /// Contact for handling product requests.
    /// </summary>
    public interface IProductRequestService
    {
        Task<ResponseWrapper> ProcessAnalysisRequestAsync(Guid userId, AnalysisInputModel inputModel, CancellationToken token);

        Task<ResponseWrapper> ProcessStatisticsRequestAsync(Guid userId, StatisticsInputModel inputModel, CancellationToken token);
    }
}
