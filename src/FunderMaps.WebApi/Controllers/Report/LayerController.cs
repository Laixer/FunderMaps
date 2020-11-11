using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for layer operations.
    /// </summary>
    [Route("layer")]
    public class LayerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILayerRepository _layerRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerController(IMapper mapper, ILayerRepository layerRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
        }

        // GET: api/layer/{id}
        /// <summary>
        ///     Return layer by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            // Act.
            var layer = await _layerRepository.GetByIdAsync(id);

            // Map.
            var output = _mapper.Map<LayerDto>(layer);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
