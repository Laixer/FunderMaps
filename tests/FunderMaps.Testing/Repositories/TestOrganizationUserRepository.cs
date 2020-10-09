using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
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

        public ValueTask AddAsync(Guid organizationId, Guid userId, OrganizationRole role)
        {
            DataStore.Add(new OrganizationUserRecord
            {
                UserId = userId,
                OrganizationId = organizationId,
                OrganizationRole = role,
            });
            return new ValueTask();
        }

        public ValueTask<bool> IsUserInOrganization(Guid organizationId, Guid userId)
            => new ValueTask<bool>(DataStore.ItemList.Any(e => e.UserId == userId && e.OrganizationId == organizationId));

        public IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId, INavigation navigation)
            => Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Select(s => s.UserId), navigation));

        public IAsyncEnumerable<Guid> ListAllByRoleAsync(Guid organizationId, OrganizationRole[] organizationRole, INavigation navigation)
            => Helper.AsAsyncEnumerable(Helper.ApplyNavigation(DataStore.ItemList.Where(s => organizationRole.Contains(s.OrganizationRole)).Select(s => s.UserId), navigation));

        public ValueTask<Guid> GetOrganizationByUserIdAsync(Guid userId)
            => new ValueTask<Guid>(DataStore.ItemList.First(e => e.UserId == userId).OrganizationId);

        public ValueTask<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId)
            => new ValueTask<OrganizationRole>(DataStore.ItemList.First(e => e.UserId == userId).OrganizationRole);
    }
}
