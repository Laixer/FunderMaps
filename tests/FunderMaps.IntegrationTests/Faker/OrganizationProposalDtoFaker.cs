using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class OrganizationProposalDtoFaker : Faker<OrganizationProposalDto>
    {
        public OrganizationProposalDtoFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Uuid());
            RuleFor(f => f.Name, f => f.Company.CompanyName());
            RuleFor(f => f.Email, f => f.Internet.Email());
        }
    }
}
