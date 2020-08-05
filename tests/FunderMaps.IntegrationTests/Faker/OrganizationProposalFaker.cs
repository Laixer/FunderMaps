using Bogus;
using FunderMaps.Core.Entities;

namespace FunderMaps.IntegrationTests.Faker
{
    public class OrganizationProposalFaker : Faker<OrganizationProposal>
    {
        public OrganizationProposalFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Uuid());
            RuleFor(f => f.Name, f => f.Company.CompanyName());
            RuleFor(f => f.Email, (f, o) => f.Internet.Email(provider: o.Name));
        }
    }
}
