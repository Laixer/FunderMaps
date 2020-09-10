using Bogus;
using FunderMaps.Core.Entities;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="OrganizationProposal"/>.
    /// </summary>
    public class OrganizationProposalFaker : Faker<OrganizationProposal>
    {
        public OrganizationProposalFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Uuid());
            RuleFor(f => f.Name, f => f.Company.CompanyName());
            RuleFor(f => f.Email, (f, o) => f.Internet.Email());
        }
    }
}
