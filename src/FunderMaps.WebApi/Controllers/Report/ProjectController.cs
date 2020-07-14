using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if DEBUG || USE_PROJECT // NOTE: This is a future feature already implemented.

namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    /// Endpoint controller for project operations.
    /// </summary>
    [ApiController]
    [Route("api/project")]
    public class ProjectController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ProjectUseCase _projectUseCase;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public ProjectController(IMapper mapper, ProjectUseCase projectUseCase)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _projectUseCase = projectUseCase ?? throw new ArgumentNullException(nameof(projectUseCase));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var project = await _projectUseCase.GetAsync(id).ConfigureAwait(false);

            return Ok(_mapper.Map<ProjectDTO>(project));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            // FUTURE: Missing IAsyncEnum map()
            var result = new List<ProjectDTO>();
            await foreach (var item in _projectUseCase.GetAllAsync(pagination.Navigation))
            {
                result.Add(_mapper.Map<ProjectDTO>(item));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProjectDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var project = await _projectUseCase.CreateAsync(_mapper.Map<Project>(input)).ConfigureAwait(false);

            return Ok(_mapper.Map<ProjectDTO>(project));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectDTO input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var project = _mapper.Map<Project>(input);
            project.Id = id;

            await _projectUseCase.UpdateAsync(project).ConfigureAwait(false);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _projectUseCase.DeleteAsync(id).ConfigureAwait(false);

            return NoContent();
        }
    }
}

#endif
