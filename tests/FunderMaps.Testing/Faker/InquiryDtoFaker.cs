using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Types;
using FunderMaps.Testing.Extensions;
using FunderMaps.WebApi.DataTransferObjects;
using System;

namespace FunderMaps.Testing.Faker
{
    /// <summary>
    ///     Faker for <see cref="InquiryDto"/>.
    /// </summary>
    public class InquiryDtoFaker : Faker<InquiryDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InquiryDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.DocumentName, f => f.Commerce.Product());
            RuleFor(f => f.Inspection, f => f.Random.Bool());
            RuleFor(f => f.JointMeasurement, f => f.Random.Bool());
            RuleFor(f => f.FloorMeasurement, f => f.Random.Bool());
            RuleFor(f => f.Note, f => f.Lorem.Text().OrNull(f, .7f));
            RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
            RuleFor(f => f.DocumentFile, f => f.Internet.RemoteFileWithSecureUrl());
            RuleFor(f => f.Type, f => f.PickRandom<InquiryType>());
            RuleFor(f => f.StandardF3o, f => f.Random.Bool(0.3f));
            RuleFor(f => f.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.Reviewer, f => f.Random.Uuid().OrNull(f, 0.1f));
            RuleFor(f => f.Contractor, f => f.Random.Uuid());
            RuleFor(f => f.AccessPolicy, f => f.PickRandom<AccessPolicy>());
            RuleFor(f => f.CreateDate, f => DateTime.Now);
            RuleFor(f => f.UpdateDate, f => f.Date.Future());
        }
    }
}
