using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.Webservice.Controllers
{
    /// <summary>
    ///     Controller for all product endpoints.
    /// </summary>
    [Obsolete("See ProductV2Controller")]
    [Route("product")]
    public sealed class ProductController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProductController(IMapper mapper, IProductService productService)
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

        /// <summary>
        ///     Builds the output object associated with the product.
        /// </summary>
        internal async Task<ResponseWrapper> AnalysisProductFactoryAsync(AnalysisProductType productType, IAsyncEnumerable<AnalysisProduct> itemList)
            => productType switch
            {
                AnalysisProductType.Foundation => await AsResponseWrapperAsync<AnalysisFoundationDto, AnalysisProduct>(itemList),
                AnalysisProductType.Complete => await AsResponseWrapperAsync<AnalysisCompleteDto, AnalysisProduct>(itemList),
                AnalysisProductType.RiskPlus => await AsResponseWrapperAsync<AnalysisRiskPlusDto, AnalysisProduct>(itemList),
                _ => throw new InvalidOperationException(nameof(productType)),
            };

        // GET: api/product/analysis
        /// <summary>
        ///     Request an analysis product.
        /// </summary>
        [HttpGet("analysis")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseWrapper<AnalysisDto>>> GetProductAnalysisAsync([FromQuery] AnalysisInputModel input)
        {
            // Assign.
            IAsyncEnumerable<AnalysisProduct> productList = _productService.GetAnalysisAsync(input.Product.Value, input.Id);

            // Map.
            ResponseWrapper result = await AnalysisProductFactoryAsync(input.Product.Value, productList);

            // Return.
            return Ok(result);
        }

        // GET: api/product/statistics
        /// <summary>
        ///     Request statistics product.
        /// </summary>
        [HttpGet("statistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseWrapper<StatisticsDto>>> GetProductStatisticsAsync([FromQuery][Required] string id)
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
#pragma warning restore CA1062 // Validate arguments of public methods
