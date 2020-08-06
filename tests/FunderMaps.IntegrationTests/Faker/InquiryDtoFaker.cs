using Bogus;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.WebApi.DataTransferObjects;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    public class InquiryDtoFaker : Faker<InquiryDto>
    {
        public InquiryDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.DocumentName, f => f.Commerce.Product());
            RuleFor(f => f.Inspection, f => f.Random.Bool());
            RuleFor(f => f.JointMeasurement, f => f.Random.Bool());
            RuleFor(f => f.FloorMeasurement, f => f.Random.Bool());
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
            RuleFor(f => f.DocumentFile, f => f.Internet.RemoteFileWithSecureUrl());
            RuleFor(f => f.Type, f => f.PickRandom<InquiryType>());
            RuleFor(f => f.StandardF3o, f => f.Random.Bool(0.3f));
            RuleFor(f => f.Attribution, f => f.Random.Int(0, int.MaxValue)); // TODO
            RuleFor(f => f.AccessPolicy, f => f.PickRandom<AccessPolicy>());
        }
    }
}
