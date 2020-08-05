using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Entities;

namespace FunderMaps.IntegrationTests.Faker
{
    public class ProjectFaker : Faker<Project>
    {
        public User Adviser { get; } = new UserFaker().Generate();
        public User Lead { get; } = new UserFaker().Generate();
        public User Creator { get; } = new UserFaker().Generate();

        public ProjectFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Dossier, f => f.Commerce.Product());
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.StartDate, f => f.Date.Past(10));
            RuleFor(f => f.EndDate, f => f.Date.Past(10).OrNull(f, 0.15f));
            RuleFor(f => f.Adviser, f => Adviser.Id.OrNull(f));
            RuleFor(f => f.Lead, f => Lead.Id.OrNull(f));
            RuleFor(f => f.Creator, f => Creator.Id.OrNull(f));
        }
    }
}
