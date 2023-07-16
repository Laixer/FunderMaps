using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

namespace FunderMaps.IntegrationTests.Faker
{
    /// <summary>
    ///     Faker for <see cref="Recovery"/>.
    /// </summary>
    public class RecoveryFaker : Faker<Recovery>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public RecoveryFaker()
        {
            RuleFor(f => f.Id, f => f.UniqueIndex);
            RuleFor(f => f.DocumentName, f => f.System.FileName());
            RuleFor(f => f.Note, f => f.Lorem.Text().OrNull(f, .8f));
            RuleFor(f => f.Type, f => f.PickRandom<RecoveryDocumentType>());
            RuleFor(f => f.DocumentFile, f => f.System.FileName());
            RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
            RuleFor(f => f.State.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.Attribution.Reviewer, f => f.Random.Uuid().OrNull(f, 0.2f));
            RuleFor(f => f.Attribution.Contractor, f => f.Random.Int(0, 10_000));
            RuleFor(f => f.Access.AccessPolicy, f => f.PickRandom<AccessPolicy>());
        }
    }
}
