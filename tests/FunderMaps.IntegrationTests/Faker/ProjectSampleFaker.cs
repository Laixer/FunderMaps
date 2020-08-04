using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Faker
{
    public class ProjectSampleFaker : Faker<ProjectSample>
    {
        public Project Project { get; } = new ProjectFaker().Generate();
        public Address Address { get; } = new AddressFaker().Generate();
        public Contact Contact { get; } = new ContactFaker().Generate();

        public ProjectSampleFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Project, f => Project.Id);
            RuleFor(f => f.Status, f => f.PickRandom<ProjectSampleStatus>());
            RuleFor(f => f.Email, f => Contact.Email);
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Address, f => Address.Id);
        }
    }
}
