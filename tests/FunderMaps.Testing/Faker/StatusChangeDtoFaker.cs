using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="StatusChangeDto"/>.
    /// </summary>
    public class StatusChangeDtoFaker : Faker<StatusChangeDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public StatusChangeDtoFaker()
        {
            RuleFor(f => f.Message, f => f.Lorem.Text());
        }
    }
}
