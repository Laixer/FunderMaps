using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    internal class ProjectSampleRepository : RepositoryBase<ProjectSample, int>, IProjectSampleRepository
    {
        public override ValueTask<int> AddAsync(ProjectSample entity)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<ulong> CountAsync()
        {
            throw new NotImplementedException();
        }

        public override ValueTask DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override ValueTask<ProjectSample> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public override IAsyncEnumerable<ProjectSample> ListAllAsync(INavigation navigation)
        {
            throw new NotImplementedException();
        }

        public override ValueTask UpdateAsync(ProjectSample entity)
        {
            throw new NotImplementedException();
        }
    }
}
