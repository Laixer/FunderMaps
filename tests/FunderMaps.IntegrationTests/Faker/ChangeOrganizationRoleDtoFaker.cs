using Bogus;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Types;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="ChangeOrganizationRoleDto"/>.
    /// </summary>
    public class ChangeOrganizationRoleDtoFaker : Faker<ChangeOrganizationRoleDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ChangeOrganizationRoleDtoFaker()
        {
            RuleFor(f => f.Role, f => f.PickRandom<OrganizationRole>());
        }
    }
}
