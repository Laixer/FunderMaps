using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.UseCases;
using FunderMaps.WebApi.DataTransferObjects;
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

        // GET: api/project/{id}
        /// <summary>
        ///     Return project by id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var project = await _projectUseCase.GetAsync(id);

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        // GET: api/project
        /// <summary>
        ///     Return all projects.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            var result = await _mapper.MapAsync<IList<ProjectDto>, Project>(_projectUseCase.GetAllAsync(pagination.Navigation));

            return Ok(result);
        }

        // POST: api/project
        /// <summary>
        ///     Create project.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProjectDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var project = await _projectUseCase.CreateAsync(_mapper.Map<Project>(input));

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        // PUT: api/project
        /// <summary>
        ///     Create project.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ProjectDto input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var project = _mapper.Map<Project>(input);
            project.Id = id;

            await _projectUseCase.UpdateAsync(project);

            return NoContent();
        }

        // DELETE: api/project/{id}
        /// <summary>
        ///     Delete project by id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _projectUseCase.DeleteAsync(id);

            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods

#endif
