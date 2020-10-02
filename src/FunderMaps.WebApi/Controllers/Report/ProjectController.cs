using AutoMapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if DEBUG || USE_PROJECT // NOTE: This is a future feature already implemented.

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Report
{
    /// <summary>
    ///     Endpoint controller for project operations.
    /// </summary>
    [Route("project")]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ProjectUseCase _projectUseCase;

        /// <summary>
        ///     Create new instance.
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

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<ProjectDto>, Project>(_projectUseCase.GetAllAsync(pagination.Navigation))
                .ConfigureAwait(false);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProjectDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var project = await _projectUseCase.CreateAsync(_mapper.Map<Project>(input)).ConfigureAwait(false);

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectDto input)
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
#pragma warning restore CA1062 // Validate arguments of public methods

#endif
