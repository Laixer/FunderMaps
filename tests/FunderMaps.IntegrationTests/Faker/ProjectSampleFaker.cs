using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

namespace FunderMaps.IntegrationTests.Faker
{
    public class ProjectSampleFaker : Faker<ProjectSample>
    {
        public ProjectSampleFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Project, f => new ProjectFaker().Generate().Id);
            RuleFor(f => f.Status, f => f.PickRandom<ProjectSampleStatus>());
            RuleFor(f => f.Email, f => new ContactFaker().Generate().Email);
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Address, f => new AddressFaker().Generate().Id);
        }
    }
}
