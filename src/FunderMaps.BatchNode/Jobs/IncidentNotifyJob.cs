using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FunderMaps.BatchNode.Jobs.Notification;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Threading;
using Scriban.Runtime;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.BatchNode.Jobs
{
    /// <summary>
    ///     FooBar dummy job.
    /// </summary>
    internal class IncidentNotifyJob : EmailJob
    {
        private const string TaskName = "INCIDENT_NOTIFY";

        private readonly ITemplateParser _templateParser;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IContactRepository _contactRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentNotifyJob(
            ITemplateParser templateParser,
            IIncidentRepository incidentRepository,
            IAddressRepository addressRepository,
            IContactRepository contactRepository,
            IEmailService emailService)
            : base(emailService)
        {
            _templateParser = templateParser;
            _incidentRepository = incidentRepository;
            _addressRepository = addressRepository;
            _contactRepository = contactRepository;
        }

        // TODO: Move into culture service?
        class TranslationFunctions : ScriptObject
        {
            public static string ToBoolean(bool value)
            {
                return value ? "Ja" : "Nee";
            }

            public static string ToFoundationType(Core.Types.FoundationType value)
                => value switch
                {
                    Core.Types.FoundationType.Wood => "Hout",
                    Core.Types.FoundationType.WoodAmsterdam => "Hout",
                    Core.Types.FoundationType.WoodRotterdam => "Hout",
                    Core.Types.FoundationType.WoodCharger => "Hout",
                    Core.Types.FoundationType.Concrete => "Beton",
                    Core.Types.FoundationType.NoPile => "Niet onderheid",
                    Core.Types.FoundationType.NoPileMasonry => "Niet onderheid",
                    Core.Types.FoundationType.NoPileStrips => "Niet onderheid",
                    Core.Types.FoundationType.NoPileBearingFloor => "Niet onderheid",
                    Core.Types.FoundationType.NoPileConcreteFloor => "Niet onderheid",
                    Core.Types.FoundationType.NoPileSlit => "Niet onderheid",
                    Core.Types.FoundationType.WeightedPile => "Beton met verzwaardepunt",
                    Core.Types.FoundationType.Combined => "Gecombineerd",
                    Core.Types.FoundationType.SteelPile => "Stalen buispaal",
                    Core.Types.FoundationType.Other => "Overig",
                    _ => "Onbekend",
                };

            public static string ToFoundationDamageCause(Core.Types.FoundationDamageCause value)
                => value switch
                {
                    Core.Types.FoundationDamageCause.Drainage => "Ontwateringsdiepte onvoldoende",
                    Core.Types.FoundationDamageCause.Drystand => "Droogstand",
                    Core.Types.FoundationDamageCause.ConstructionFlaw => "Verkeerd gefundeerd",
                    Core.Types.FoundationDamageCause.Overcharge => "Overbelasting",
                    Core.Types.FoundationDamageCause.OverchargeNegativeCling => "Overbelasting/Negatievekleef",
                    Core.Types.FoundationDamageCause.NegativeCling => "Negatievekleef",
                    Core.Types.FoundationDamageCause.BioInfection => "Bacteriele aantasting",
                    Core.Types.FoundationDamageCause.FungusInfection => "Schimmelaantasting",
                    Core.Types.FoundationDamageCause.BioFungusInfection => "Schimmel/bacterieen",
                    Core.Types.FoundationDamageCause.FoundationFlaw => "Verkeerd gefundeerd",
                    Core.Types.FoundationDamageCause.ConstructionHeave => "Woning omhooggedrukt",
                    Core.Types.FoundationDamageCause.Subsidence => "Bodemdaling",
                    Core.Types.FoundationDamageCause.Vegetation => "Wortels/planten",
                    Core.Types.FoundationDamageCause.Gas => "Gaswinning/mijnbouw",
                    Core.Types.FoundationDamageCause.Vibrations => "Verkeer",
                    Core.Types.FoundationDamageCause.PartialFoundationRecovery => "Naastgelegen funderingsherstel",
                    _ => "Onbekend",
                };

            public static string ToFoundationDamageCharacteristics(Core.Types.FoundationDamageCharacteristics value)
                => value switch
                {
                    Core.Types.FoundationDamageCharacteristics.JammingDoorWindow => "Klemmende ramen/deuren",
                    Core.Types.FoundationDamageCharacteristics.Crack => "Scheuren",
                    Core.Types.FoundationDamageCharacteristics.Skewed => "Scheefstand",
                    Core.Types.FoundationDamageCharacteristics.CrawlspaceFlooding => "Water in kruipruimte",
                    Core.Types.FoundationDamageCharacteristics.ThresholdAboveSubsurface => "Maaiveld lager dan dorpel",
                    Core.Types.FoundationDamageCharacteristics.ThresholdBelowSubsurface => "Drempel lager dan maaiveld",
                    Core.Types.FoundationDamageCharacteristics.CrookedFloorWall => "Scheven vloer",
                    _ => "Onbekend",
                };

            public static IEnumerable<string> ArrayToFoundationDamageCharacteristics(IEnumerable<Core.Types.FoundationDamageCharacteristics> values)
            {
                Collection<string> list = new();
                foreach (var value in values)
                {
                    list.Add(ToFoundationDamageCharacteristics(value));
                }
                return list;
            }

            public static string ToEnvironmentDamageCharacteristics(Core.Types.EnvironmentDamageCharacteristics value)
                => value switch
                {
                    Core.Types.EnvironmentDamageCharacteristics.Subsidence => "Bodemdaling",
                    Core.Types.EnvironmentDamageCharacteristics.SaggingSewerConnection => "Verzakkend riool",
                    Core.Types.EnvironmentDamageCharacteristics.SaggingCablesPipes => "Verzakkende kabels/leidingen",
                    Core.Types.EnvironmentDamageCharacteristics.Flooding => "Wateroverlast",
                    Core.Types.EnvironmentDamageCharacteristics.FoundationDamageNearby => "Funderingschade in wijk",
                    Core.Types.EnvironmentDamageCharacteristics.Elevation => "Recent opgehoogd",
                    Core.Types.EnvironmentDamageCharacteristics.IncreasingTraffic => "Verkeerstoename",
                    Core.Types.EnvironmentDamageCharacteristics.ConstructionNearby => "Werkzaamheden in wijk",
                    Core.Types.EnvironmentDamageCharacteristics.VegetationNearby => "Bomen nabij",
                    Core.Types.EnvironmentDamageCharacteristics.SewageLeakage => "Lekkend riool",
                    Core.Types.EnvironmentDamageCharacteristics.LowGroundWater => "Wateronderlast",
                    _ => "Onbekend",
                };

            public static IEnumerable<string> ArrayToEnvironmentDamageCharacteristics(IEnumerable<Core.Types.EnvironmentDamageCharacteristics> values)
            {
                Collection<string> list = new();
                foreach (var value in values)
                {
                    list.Add(ToEnvironmentDamageCharacteristics(value));
                }
                return list;
            }
        }

        // FUTURE: It makes much more sense to have the templateParser be injected in the
        //         email service. Most if not all email will use some sort of HTML template.
        /// <summary>
        ///     Handle the incoming notification.
        /// </summary>
        /// <param name="context">Notification context.</param>
        /// <param name="envelope">Envelope containing the notification.</param>
        public override async Task NotifyAsync(BackgroundTaskContext context, Envelope envelope)
        {
            Incident incident = await _incidentRepository.GetByIdAsync(envelope.Items["id"]);
            Address address = await _addressRepository.GetByIdAsync(incident.Address);

            _templateParser.AddObject(nameof(incident), incident);
            _templateParser.AddObject(nameof(address), address);

            if (!string.IsNullOrEmpty(incident.Email))
            {
                var contact = await _contactRepository.GetByIdAsync(incident.Email);
                _templateParser.AddObject(nameof(contact), contact);
            }

            _templateParser.FromTemplateFile("Email", "incident");

            // FUTURE: Fix this cast.
            (_templateParser as Core.Components.TemplateParser).RegisterExtension(new TranslationFunctions());

            envelope.Content = await _templateParser.RenderAsync(context.CancellationToken);
            envelope.Subject = $"Nieuwe melding: {incident.Id}";

            await base.NotifyAsync(context, envelope);
        }

        /// <summary>
        ///     Method to check if a task can be handled by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToUpperInvariant() == TaskName && CanHandleEnvelope(name, value);
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
