using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="ProjectSample"/>.
    /// </summary>
    public class ProjectSampleFaker : Faker<ProjectSample>
    {
        public ProjectSampleFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.Project, f => f.UniqueIndex);
            RuleFor(f => f.Status, f => f.PickRandom<ProjectSampleStatus>());
            RuleFor(f => f.Email, f => f.Internet.Email());
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.Address, f => $"gfm-{f.Random.Hash(32)}");
        }
    }
}
