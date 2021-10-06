using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.Controllers
{
    // FUTURE: Split logic into analysis, risk index, statistics controller.
    // FUTURE: Drop the 'product' uri prefix.
    // FUTURE: Drop the 'v' uri version prefix.
    /// <summary>
    ///     Controller for all product endpoints.
    /// </summary>
    [Route("v2/product")]
    public sealed class ProductV2Controller : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductV2Controller(IMapper mapper, IProductService productService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <summary>
        ///     Encapulate transfer object into item wrapper.
        /// </summary>
        private async Task<ResponseWrapper<TDto>> AsResponseWrapperAsync<TDto, TSource>(IAsyncEnumerable<TSource> itemList)
            => new()
            {
                Items = await _mapper.MapAsync<IList<TDto>, TSource>(itemList)
            };

        // GET: api/v2/product/analysis
        /// <summary>
        ///     Request the analysis product.
        /// </summary>
        [HttpGet("analysis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<AnalysisV2Dto>>> GetAnalysisAsync([FromQuery] string id)
        {
            // Assign.
            IAsyncEnumerable<AnalysisProduct2> productList = _productService.GetAnalysis2Async(id);

            // Map.
            var result = await _mapper.MapAsync<IList<AnalysisV2Dto>, AnalysisProduct2>(productList);

            // Return.
            return Ok(result);
        }

        // GET: api/v2/product/at_risk
        /// <summary>
        ///     Request the risk index per id.
        /// </summary>
        [HttpGet("at_risk")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<bool> GetRiskIndexAsync([FromQuery] string id)
            => _productService.GetRiskIndexAsync(id);

        // GET: api/v2/product/statistics
        /// <summary>
        ///     Request the statistics product.
        /// </summary>
        [HttpGet("statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseWrapper<StatisticsDto>>> GetStatisticsAsync([FromQuery][Required] string id)
        {
            // Assign.
            IAsyncEnumerable<StatisticsProduct> productList = _productService.GetStatisticsAsync(id);

            // Map.
            ResponseWrapper result = await AsResponseWrapperAsync<StatisticsDto, StatisticsProduct>(productList);

            // Return.
            return Ok(result);
        }
    }
}
