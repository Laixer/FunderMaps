using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Project repository.
    /// </summary>
    internal class ProjectRepository : RepositoryBase<Project, int>, IProjectRepository
    {
        public override ValueTask<int> AddAsync(Project entity)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<long> CountAsync()
        {
            throw new NotImplementedException();
        }

        public override ValueTask DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<Project> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override IAsyncEnumerable<Project> ListAllAsync(INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public override ValueTask UpdateAsync(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
