using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Abstractions.Services;
using FunderMaps.Webservice.ResponseModels;
using System;
using System.Threading.Tasks;
using FunderMaps.Core.Extensions;
using FunderMaps.Webservice.Utility;
using FunderMaps.Core.Types;
using FunderMaps.Webservice.Translation;
using AutoMapper;
using FunderMaps.Webservice.ResponseModels.Analysis;
using System.Collections.Generic;
using FunderMaps.Webservice.Mapping;

namespace FunderMaps.Webservice.Services
{
    /// <summary>
    /// Manages product requests, including conversion to response models.
    /// </summary>
    public sealed class ProductResultService : IProductResultService
    {
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public ProductResultService(IProductService productService, IMappingService mappingService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _mappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
        }

        public async Task<ResponseWrapper> GetAnalysisByBagIdAsync(Guid userId, AnalysisProductType productType, string bagId, uint pageNumber = 1, uint pageCount = 25)
        {
            userId.ThrowIfNullOrEmpty();
            bagId.ThrowIfNullOrEmpty();
            if (pageNumber == 0) { throw new ArgumentOutOfRangeException(nameof(pageNumber)); }
            if (pageCount == 0) { throw new ArgumentOutOfRangeException(nameof(pageCount)); }

            var result = await _productService.GetAnalysisByExternalIdAsync(userId, productType, bagId, ExternalDataSource.Bag, pageNumber, pageCount).ConfigureAwait(false);
            return _mappingService.MapToAnalysisWrapper(productType, new List<AnalysisProduct> { result });
        }

        public Task<ResponseWrapper> GetAnalysisByIdAsync(Guid userId, AnalysisProductType productType, string id, uint pageNumber = 1, uint pageCount = 25) => throw new NotImplementedException();

        public Task<ResponseWrapper> GetAnalysisByQueryAsync(Guid userId, AnalysisProductType productType, string query, uint pageNumber = 1, uint pageCount = 25) => throw new NotImplementedException();

        public Task<ResponseWrapper> GetAnalysisInFenceAsync(Guid userId, AnalysisProductType productType, uint pageNumber = 1, uint pageCount = 25) => throw new NotImplementedException();

        public Task<ResponseWrapper> GetStatisticsByAreaAsync(Guid userId, StatisticsProductType productType, string areaCode, uint pageNumber = 1, uint pageCount = 25) => throw new NotImplementedException();

        public Task<ResponseWrapper> GetStatisticsInFenceAsync(Guid userId, StatisticsProductType productType, uint pageNumber = 1, uint pageCount = 25) => throw new NotImplementedException();
    }
}
