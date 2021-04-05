using Bogus;
using Bogus.DataSets;
using Bogus.Extensions;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="RecoveryDto"/>.
    /// </summary>
    public class RecoveryDtoFaker : Faker<RecoveryDto>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryDtoFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.DocumentName, f => f.System.FileName());
            RuleFor(f => f.Note, f => f.Lorem.Text().OrNull(f, .8f));
            RuleFor(f => f.Type, f => f.PickRandom<RecoveryDocumentType>());
            RuleFor(f => f.DocumentFile, f => f.System.FileName());
            RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
            RuleFor(f => f.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.Reviewer, f => f.Random.Uuid().OrNull(f, 0.2f));
            RuleFor(f => f.Contractor, f => f.Random.Uuid());
            RuleFor(f => f.AccessPolicy, f => f.PickRandom<AccessPolicy>());
        }
    }
}
