using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for incident operations.
    /// </summary>
    [Route("bundle")]
    public class BundleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBundleRepository _bundleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleController(
            IMapper mapper,
            IBundleRepository bundleRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
        }

        // GET: api/bundles
        /// <summary>
        ///     Return all bundles.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            var bundleList = new List<Bundle>();
            await foreach (var bundle in _bundleRepository.ListAllAsync(pagination.Navigation))
            {
                bundleList.Add(bundle);
            }

            // Map.
            var output = _mapper.Map<IList<BundleDto>>(bundleList);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
