using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class StatusChangeDtoFaker : Faker<StatusChangeDto>
    {
        public StatusChangeDtoFaker()
        {
            RuleFor(f => f.Message, f => f.Lorem.Text());
        }
    }
}
