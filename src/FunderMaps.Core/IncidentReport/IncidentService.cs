using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Core.IncidentReport
{
    // FUTURE: Revamp this service.
    /// <summary>
    ///     Service to the incidents.
    /// </summary>
    internal class IncidentService : IIncidentService // TODO: inherit from AppServiceBase
    {
        private readonly IncidentOptions _options;
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IGeocoderTranslation _geocoderTranslation;
        private readonly INotifyService _notifyService;
        private readonly ILogger<IncidentService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentService(
            IOptions<IncidentOptions> options,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IGeocoderTranslation geocoderTranslation,
            INotifyService notificationService,
            ILogger<IncidentService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _geocoderTranslation = geocoderTranslation ?? throw new ArgumentNullException(nameof(geocoderTranslation));
            _notifyService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // TODO: This is a temporary solution.
        // TODO: Move into culture service?
        class TranslationFunctions : ScriptObject
        {
            public static string ToBoolean(bool value) => value ? "Ja" : "Nee";

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
                    Core.Types.FoundationDamageCharacteristics.CrookedFloorWall => "Scheve vloer",
                    _ => "Onbekend",
                };

            public static IEnumerable<string> ArrayToFoundationDamageCharacteristics(IEnumerable<Core.Types.FoundationDamageCharacteristics> values)
                => values.Select(value => ToFoundationDamageCharacteristics(value));

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
                => values.Select(value => ToEnvironmentDamageCharacteristics(value));
        }

        // FUTURE: split logic, hard to read.
        /// <summary>
        ///     Register a new incident.
        /// </summary>
        /// <param name="incident">Incident to process.</param>
        /// <param name="meta">Optional metadata.</param>
        public async Task<Incident> AddAsync(Incident incident, object meta = null)
        {
            Address address = await _geocoderTranslation.GetAddressIdAsync(incident.Address);

            incident.Address = address.Id;
            incident.ClientId = _options.ClientId;
            incident.Meta = meta;
            incident.AuditStatus = AuditStatus.Todo;

            await _contactRepository.AddAsync(incident.ContactNavigation);
            incident = await _incidentRepository.AddGetAsync(incident);
            incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);

            List<string> recipients = new(_options.Recipients);
            recipients.Add(incident.Email);

            string subject = $"Nieuwe melding: {incident.Id}";

            object header = new
            {
                Title = subject,
                Preheader = "Dit is een kopie van de door u verzonden melding in het funderingsloket."
            };

            string footer = "Dit bericht wordt verstuurd wanneer een melding binnenkomt op het loket.";

            await _notifyService.NotifyAsync(new()
            {
                Recipients = recipients,
                Subject = subject,
                Template = "Incident",
                Items = new Dictionary<string, object>
                {
                    { "header", header },
                    { "footer", footer },
                    { "incident", incident },
                    { "address", address },
                    { "contact", incident.ContactNavigation },
                },
                Extensions = new List<object> { new TranslationFunctions() },
            });

            _logger.LogInformation($"New incident from {incident.Email} was recorded.");

            return incident;
        }
    }
}
