using Bogus;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    public class IncidentFaker : Faker<Incident>
    {
        public IncidentFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Replace("FIR######-#####"));
            RuleFor(f => f.ClientId, f => f.Random.Number(1, 99));
            RuleFor(f => f.FoundationType, f => f.PickRandom<FoundationType>());
            RuleFor(f => f.ChainedBuilding, f => f.Random.Bool());
            RuleFor(f => f.Owner, f => f.Random.Bool());
            RuleFor(f => f.FoundationRecovery, f => f.Random.Bool());
            RuleFor(f => f.NeightborRecovery, f => f.Random.Bool());
            RuleFor(f => f.FoundationDamageCause, f => f.PickRandom<FoundationDamageCause>());
            RuleFor(f => f.DocumentFile, f => new string[] { f.Internet.UrlWithPath("https", f.Internet.DomainName(), ".pdf") }); // TODO: 0-n
            RuleFor(f => f.Note, f => f.Lorem.Text());
            RuleFor(f => f.InternalNote, f => f.Lorem.Text());
            RuleFor(f => f.FoundationDamageCharacteristics, f => f.Random.ArrayElements((FoundationDamageCharacteristics[])Enum.GetValues(typeof(FoundationDamageCharacteristics))));
            RuleFor(f => f.EnvironmentDamageCharacteristics, f => f.Random.ArrayElements((EnvironmentDamageCharacteristics[])Enum.GetValues(typeof(EnvironmentDamageCharacteristics))));
            RuleFor(f => f.Email, f => f.Internet.Email());
            RuleFor(f => f.Address, f => AddressFaker.AddressId);
            RuleFor(f => f.AuditStatus, f => f.PickRandom<AuditStatus>());
            RuleFor(f => f.Meta, f => new { Gateway = f.Commerce.Product() });
            RuleFor(f => f.QuestionType, f => f.PickRandom<IncidentQuestionType>());
        }
    }
}
