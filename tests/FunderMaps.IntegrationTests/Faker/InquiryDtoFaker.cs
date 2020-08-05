using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class InquiryDtoFaker : Faker<InquiryDto>
    {
        public InquiryDtoFaker()
        {
            RuleFor(f => f.Id, f => new InquiryFaker().Generate().Id);
            RuleFor(f => f.DocumentName, f => new InquiryFaker().Generate().DocumentName);
            RuleFor(f => f.Inspection, f => new InquiryFaker().Generate().Inspection);
            RuleFor(f => f.JointMeasurement, f => new InquiryFaker().Generate().JointMeasurement);
            RuleFor(f => f.FloorMeasurement, f => new InquiryFaker().Generate().FloorMeasurement);
            RuleFor(f => f.Note, f => new InquiryFaker().Generate().Note);
            RuleFor(f => f.DocumentDate, f => new InquiryFaker().Generate().DocumentDate);
            RuleFor(f => f.DocumentFile, f => new InquiryFaker().Generate().DocumentFile);
            RuleFor(f => f.Type, f => new InquiryFaker().Generate().Type);
            RuleFor(f => f.StandardF3o, f => new InquiryFaker().Generate().StandardF3o);
            RuleFor(f => f.Attribution, f => f.Random.Int(0, int.MaxValue)); // TODO
            RuleFor(f => f.AccessPolicy, f => f.PickRandom<AccessPolicy>());
        }
    }
}
