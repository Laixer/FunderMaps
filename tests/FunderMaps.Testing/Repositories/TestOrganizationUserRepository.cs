using FunderMaps.Core;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    public class OrganizationUserRecord
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public OrganizationRole OrganizationRole { get; set; }
    }

    public class TestOrganizationUserRepository : IOrganizationUserRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public DataStore<OrganizationUserRecord> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestOrganizationUserRepository(DataStore<OrganizationUserRecord> dataStore)
        {
            DataStore = dataStore;
        }

        public Task AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
        {
            DataStore.Add(new OrganizationUserRecord
            {
                UserId = userId,
                OrganizationId = organizationId,
                OrganizationRole = role,
            });
            return Task.CompletedTask;
        }

        public Task<bool> IsUserInOrganization(Guid organizationId, Guid userId)
            => Task.FromResult<bool>(DataStore.ItemList.Any(e => e.UserId == userId && e.OrganizationId == organizationId));

        public IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId, Navigation navigation)
            => Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Select(s => s.UserId), navigation));

        public IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] organizationRole, Navigation navigation)
            => Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Where(s => organizationRole.Contains(s.OrganizationRole)).Select(s => s.UserId), navigation));

        public Task<Guid> GetOrganizationByUserIdAsync(Guid userId)
            => Task.FromResult<Guid>(DataStore.ItemList.First(e => e.UserId == userId).OrganizationId);

        public Task<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId)
            => Task.FromResult<OrganizationRole>(DataStore.ItemList.First(e => e.UserId == userId).OrganizationRole);

        public Task SetOrganizationRoleByUserIdAsync(Guid userId, OrganizationRole role)
        {
            DataStore.ItemList.First(e => e.UserId == userId).OrganizationRole = role;
            return Task.CompletedTask;
        }
    }
}
