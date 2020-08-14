using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Repositories
{
    public class OrganizationUserRecord : BaseEntity<OrganizationUserRecord>
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
        public OrganizationRole OrganizationRole { get; set; }

        public override int CompareTo(OrganizationUserRecord other)
            => UserId.CompareTo(other.UserId);

        public override bool Equals(OrganizationUserRecord other)
            => UserId == other.UserId && OrganizationId == other.OrganizationId;
    }

    public class TestOrganizationUserRepository : IOrganizationUserRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public EntityDataStore<OrganizationUserRecord> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestOrganizationUserRepository(EntityDataStore<OrganizationUserRecord> dataStore)
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
        {
            return new ValueTask<bool>(DataStore.Entities.Any(e => e.UserId == userId && e.OrganizationId == organizationId));
        }

        public IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId)
        {
            return Helper.AsAsyncEnumerable(DataStore.Entities.Select(s => s.UserId));
        }

        public ValueTask<Guid> GetOrganizationByUserIdAsync(Guid userId)
        {
            return new ValueTask<Guid>(DataStore.Entities.First(e => e.UserId == userId).OrganizationId);
        }

        public ValueTask<OrganizationRole> GetOrganizationRoleByUserIdAsync(Guid userId)
        {
            return new ValueTask<OrganizationRole>(DataStore.Entities.First(e => e.UserId == userId).OrganizationRole);
        }
    }
}
