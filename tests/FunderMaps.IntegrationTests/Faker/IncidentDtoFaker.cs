using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class IncidentDtoFaker : Faker<IncidentDto>
    {
        public IncidentDtoFaker()
        {
            RuleFor(f => f.Id, f => new IncidentFaker().Generate().Id);
            RuleFor(f => f.ClientId, f => new IncidentFaker().Generate().ClientId);
            RuleFor(f => f.FoundationType, f => new IncidentFaker().Generate().FoundationType);
            RuleFor(f => f.ChainedBuilding, f => new IncidentFaker().Generate().ChainedBuilding);
            RuleFor(f => f.Owner, f => new IncidentFaker().Generate().Owner);
            RuleFor(f => f.FoundationRecovery, f => new IncidentFaker().Generate().FoundationRecovery);
            RuleFor(f => f.NeightborRecovery, f => new IncidentFaker().Generate().NeightborRecovery);
            RuleFor(f => f.FoundationDamageCause, f => new IncidentFaker().Generate().FoundationDamageCause);
            RuleFor(f => f.DocumentFile, f => new IncidentFaker().Generate().DocumentFile);
            RuleFor(f => f.Note, f => new IncidentFaker().Generate().Note);
            RuleFor(f => f.InternalNote, f => new IncidentFaker().Generate().InternalNote);
            RuleFor(f => f.AuditStatus, f => new IncidentFaker().Generate().AuditStatus);
            RuleFor(f => f.QuestionType, f => new IncidentFaker().Generate().QuestionType);
            RuleFor(f => f.FoundationDamageCharacteristics, f => new IncidentFaker().Generate().FoundationDamageCharacteristics);
            RuleFor(f => f.EnvironmentDamageCharacteristics, f => new IncidentFaker().Generate().EnvironmentDamageCharacteristics);
            RuleFor(f => f.Email, f => new ContactFaker().Generate().Email);
            RuleFor(f => f.Name, f => new ContactFaker().Generate().Name);
            RuleFor(f => f.PhoneNumber, f => new ContactFaker().Generate().PhoneNumber);
            RuleFor(f => f.Address, f => new AddressFaker().Generate().Id);
        }
    }
}
