using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    /// <summary>
    ///     Project use case.
    /// </summary>
    public class ProjectUseCase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectSampleRepository _projectSampleRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProjectUseCase(IProjectRepository projectRepository, IProjectSampleRepository projectSampleRepository)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _projectSampleRepository = projectSampleRepository ?? throw new ArgumentNullException(nameof(projectSampleRepository));
        }

        /// <summary>
        ///     Get project.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public async ValueTask<Project> GetAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                throw new EntityNotFoundException();
            }

            // TODO:
            //project.Advisor = ...
            //project.Lead = ...
            //project.Creator = ...

            return project;
        }

        /// <summary>
        ///     Create new project.
        /// </summary>
        /// <param name="project">Entity object.</param>
        public async ValueTask<Project> CreateAsync(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            project.InitializeDefaults();
            project.Validate();

            var id = await _projectRepository.AddAsync(project);
            return await _projectRepository.GetByIdAsync(id);
        }

        /// <summary>
        ///     Retrieve all projects.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public IAsyncEnumerable<Project> GetAllAsync(INavigation navigation)
            => _projectRepository.ListAllAsync(navigation);

        /// <summary>
        ///     Update project.
        /// </summary>
        /// <param name="project">Entity object.</param>
        public ValueTask UpdateAsync(Project project)
            => _projectRepository.UpdateAsync(project);

        /// <summary>
        ///     Delete project.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public async ValueTask DeleteAsync(int id)
        {
            await _projectRepository.DeleteAsync(id);
        }
    }
}
