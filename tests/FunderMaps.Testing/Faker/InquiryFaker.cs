using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Extensions;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="Inquiry"/>.
    /// </summary>
    public class InquiryFaker : Faker<Inquiry>
    {
        public InquiryFaker()
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
        }
    }
}
