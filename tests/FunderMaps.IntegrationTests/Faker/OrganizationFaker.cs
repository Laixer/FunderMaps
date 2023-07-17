using Bogus;
using FunderMaps.Core.Entities;

namespace FunderMaps.IntegrationTests.Faker;

/// <summary>
///     Faker for <see cref="Organization"/>.
/// </summary>
public class OrganizationFaker : Faker<Organization>
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationFaker()
    {
        RuleFor(f => f.Id, f => f.Random.Uuid());
        RuleFor(f => f.Name, f => f.Company.CompanyName());
        RuleFor(f => f.Email, (f, o) => f.Internet.Email(provider: o.Name));
    }
}
