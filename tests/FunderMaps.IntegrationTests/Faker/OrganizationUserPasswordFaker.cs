using Bogus;
using Bogus.DataSets;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Faker;

namespace FunderMaps.Testing.Faker;

/// <summary>
///     Faker for <see cref="OrganizationUserPassword"/>.
/// </summary>
// public class OrganizationUserPasswordFaker : Faker<OrganizationUserPasswordDto>
// {
//     /// <summary>
//     ///     Create new instance.
//     /// </summary>
//     public OrganizationUserPasswordDtoFaker()
//     {
//         RuleFor(f => f.Id, f => f.Random.Uuid());
//         RuleFor(f => f.GivenName, f => f.Person.FirstName.OrNull(f, .1f));
//         RuleFor(f => f.LastName, f => f.Person.LastName.OrNull(f, .1f));
//         RuleFor(f => f.Email, f => f.Person.Email);
//         RuleFor(f => f.Avatar, f => f.Person.Avatar.OrNull(f, .8f));
//         RuleFor(f => f.JobTitle, f => f.Random.ArrayElement(UserFaker.jobs.ToArray()));
//         RuleFor(f => f.PhoneNumber, f => f.Phone.PhoneNumber("###########").OrNull(f, .3f));
//         RuleFor(f => f.Role, f => f.PickRandom<ApplicationRole>());
//         RuleFor(f => f.OrganizationRole, f => f.PickRandom<OrganizationRole>());
//         RuleFor(f => f.Password, f => f.Random.Password(64));
//     }
// }
