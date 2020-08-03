using Bogus;
using FunderMaps.WebApi.DataTransferObjects;

namespace FunderMaps.IntegrationTests.Faker
{
    public class IncidentDtoFaker : Faker<IncidentDto>
    {
        public IncidentDtoFaker()
        {
            var incident = new IncidentFaker().Generate();
            var contact = new ContactFaker().Generate();
            var address = new AddressFaker().Generate();

            RuleFor(f => f.Id, f => incident.Id);
            RuleFor(f => f.ClientId, f => incident.ClientId);
            RuleFor(f => f.FoundationType, f => incident.FoundationType);
            RuleFor(f => f.ChainedBuilding, f => incident.ChainedBuilding);
            RuleFor(f => f.Owner, f => incident.Owner);
            RuleFor(f => f.FoundationRecovery, f => incident.FoundationRecovery);
            RuleFor(f => f.NeightborRecovery, f => incident.NeightborRecovery);
            RuleFor(f => f.FoundationDamageCause, f => incident.FoundationDamageCause);
            RuleFor(f => f.DocumentFile, f => new string[] { f.Internet.UrlWithPath("https", f.Internet.DomainName(), ".pdf") }); // TODO: 0-n
            RuleFor(f => f.Note, f => incident.Note);
            RuleFor(f => f.InternalNote, f => incident.InternalNote);
            RuleFor(f => f.AuditStatus, f => incident.AuditStatus);
            RuleFor(f => f.QuestionType, f => incident.QuestionType);
            RuleFor(f => f.FoundationDamageCharacteristics, f => incident.FoundationDamageCharacteristics);
            RuleFor(f => f.EnvironmentDamageCharacteristics, f => incident.EnvironmentDamageCharacteristics);
            RuleFor(f => f.Email, f => contact.Email);
            RuleFor(f => f.Name, f => contact.Name);
            RuleFor(f => f.PhoneNumber, f => contact.PhoneNumber);
            RuleFor(f => f.Address, f => address.Id);
        }
    }
}
