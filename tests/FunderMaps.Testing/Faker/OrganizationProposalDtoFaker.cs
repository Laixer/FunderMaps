using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="OrganizationProposalDto"/>.
    /// </summary>
    public class OrganizationProposalDtoFaker : Faker<OrganizationProposalDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationProposalDtoFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Uuid());
            RuleFor(f => f.Name, f => f.Company.CompanyName());
            RuleFor(f => f.Email, f => f.Internet.Email());
        }
    }
}
