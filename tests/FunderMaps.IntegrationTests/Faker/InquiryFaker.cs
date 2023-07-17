using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;

namespace FunderMaps.IntegrationTests.Faker;

/// <summary>
///     Faker for <see cref="Inquiry"/>.
/// </summary>
public class InquiryFaker : Faker<Inquiry>
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public InquiryFaker()
    {
        RuleFor(f => f.Id, f => f.UniqueIndex);
        RuleFor(f => f.DocumentName, f => f.Commerce.Product());
        RuleFor(f => f.Inspection, f => f.Random.Bool());
        RuleFor(f => f.JointMeasurement, f => f.Random.Bool());
        RuleFor(f => f.FloorMeasurement, f => f.Random.Bool());
        RuleFor(f => f.Note, f => f.Lorem.Text().OrNull(f, .7f));
        RuleFor(f => f.DocumentDate, f => f.Date.Between(DateTime.Parse("1000-01-01"), DateTime.Now));
        RuleFor(f => f.DocumentFile, f => f.System.FileName());
        RuleFor(f => f.Type, f => f.PickRandom<InquiryType>());
        RuleFor(f => f.StandardF3o, f => f.Random.Bool(0.3f));
        RuleFor(f => f.State.AuditStatus, f => f.PickRandom<AuditStatus>());
        RuleFor(f => f.Attribution.Reviewer, f => f.Random.Uuid().OrNull(f, 0.2f));
        RuleFor(f => f.Attribution.Contractor, f => f.Random.Int(0, 10_000).OrNull(f, 0.1f));
        RuleFor(f => f.Access.AccessPolicy, f => f.PickRandom<AccessPolicy>());
        RuleFor(f => f.Record.CreateDate, f => DateTime.Now);
        RuleFor(f => f.Record.UpdateDate, f => f.Date.Future());
    }
}
