using Bogus;
using Bogus.DataSets;
using FunderMaps.AspNetCore.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="ChangePasswordDto"/>.
    /// </summary>
    public class ChangePasswordDtoFaker : Faker<ChangePasswordDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ChangePasswordDtoFaker()
        {
            RuleFor(f => f.OldPassword, f => f.Random.Password(64));
            RuleFor(f => f.NewPassword, f => f.Random.Password(64));
        }
    }
}
