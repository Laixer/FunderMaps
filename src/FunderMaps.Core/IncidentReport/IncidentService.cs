using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scriban.Runtime;

namespace FunderMaps.Core.IncidentReport;

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

        public static string ToFoundationType(FoundationType? value)
            => value switch
            {
                FoundationType.Wood => "Hout",
                FoundationType.WoodAmsterdam => "Hout",
                FoundationType.WoodRotterdam => "Hout",
                FoundationType.WoodCharger => "Hout",
                FoundationType.WoodRotterdamAmsterdam => "Hout",
                FoundationType.WoodRotterdamArch => "Hout",
                FoundationType.WoodAmsterdamArch => "Hout",
                FoundationType.Concrete => "Beton",
                FoundationType.NoPile => "Niet onderheid",
                FoundationType.NoPileMasonry => "Niet onderheid",
                FoundationType.NoPileStrips => "Niet onderheid",
                FoundationType.NoPileBearingFloor => "Niet onderheid",
                FoundationType.NoPileConcreteFloor => "Niet onderheid",
                FoundationType.NoPileSlit => "Niet onderheid",
                FoundationType.WeightedPile => "Beton met verzwaardepunt",
                FoundationType.Combined => "Gecombineerd",
                FoundationType.SteelPile => "Stalen buispaal",
                FoundationType.Other => "Overig",
                _ => "Onbekend",
            };

        public static string ToFoundationDamageCause(FoundationDamageCause? value)
            => value switch
            {
                FoundationDamageCause.Drainage => "Ontwateringsdiepte onvoldoende",
                FoundationDamageCause.Drystand => "Droogstand",
                FoundationDamageCause.ConstructionFlaw => "Verkeerd gefundeerd",
                FoundationDamageCause.Overcharge => "Overbelasting",
                FoundationDamageCause.OverchargeNegativeCling => "Overbelasting/Negatievekleef",
                FoundationDamageCause.NegativeCling => "Negatievekleef",
                FoundationDamageCause.BioInfection => "Bacteriele aantasting",
                FoundationDamageCause.FungusInfection => "Schimmelaantasting",
                FoundationDamageCause.BioFungusInfection => "Schimmel/bacterieen",
                FoundationDamageCause.FoundationFlaw => "Verkeerd gefundeerd",
                FoundationDamageCause.ConstructionHeave => "Woning omhooggedrukt",
                FoundationDamageCause.Subsidence => "Bodemdaling",
                FoundationDamageCause.Vegetation => "Wortels/planten",
                FoundationDamageCause.Gas => "Gaswinning/mijnbouw",
                FoundationDamageCause.Vibrations => "Verkeer",
                FoundationDamageCause.PartialFoundationRecovery => "Naastgelegen funderingsherstel",
                _ => "Onbekend",
            };

        public static string ToFoundationDamageCharacteristics(FoundationDamageCharacteristics? value)
            => value switch
            {
                FoundationDamageCharacteristics.JammingDoorWindow => "Klemmende ramen/deuren",
                FoundationDamageCharacteristics.Crack => "Scheuren",
                FoundationDamageCharacteristics.Skewed => "Scheefstand",
                FoundationDamageCharacteristics.CrawlspaceFlooding => "Water in kruipruimte",
                FoundationDamageCharacteristics.ThresholdAboveSubsurface => "Maaiveld lager dan dorpel",
                FoundationDamageCharacteristics.ThresholdBelowSubsurface => "Drempel lager dan maaiveld",
                FoundationDamageCharacteristics.CrookedFloorWall => "Scheve vloer",
                _ => "Onbekend",
            };

        public static IEnumerable<string> ArrayToFoundationDamageCharacteristics(IEnumerable<FoundationDamageCharacteristics> values)
            => values?.Select(value => ToFoundationDamageCharacteristics(value));

        public static string ToEnvironmentDamageCharacteristics(EnvironmentDamageCharacteristics? value)
            => value switch
            {
                EnvironmentDamageCharacteristics.Subsidence => "Bodemdaling",
                EnvironmentDamageCharacteristics.SaggingSewerConnection => "Verzakkend riool",
                EnvironmentDamageCharacteristics.SaggingCablesPipes => "Verzakkende kabels/leidingen",
                EnvironmentDamageCharacteristics.Flooding => "Wateroverlast",
                EnvironmentDamageCharacteristics.FoundationDamageNearby => "Funderingschade in wijk",
                EnvironmentDamageCharacteristics.Elevation => "Recent opgehoogd",
                EnvironmentDamageCharacteristics.IncreasingTraffic => "Verkeerstoename",
                EnvironmentDamageCharacteristics.ConstructionNearby => "Werkzaamheden in wijk",
                EnvironmentDamageCharacteristics.VegetationNearby => "Bomen nabij",
                EnvironmentDamageCharacteristics.SewageLeakage => "Lekkend riool",
                EnvironmentDamageCharacteristics.LowGroundWater => "Wateronderlast",
                _ => "Onbekend",
            };

        public static IEnumerable<string> ArrayToEnvironmentDamageCharacteristics(IEnumerable<EnvironmentDamageCharacteristics> values)
            => values?.Select(value => ToEnvironmentDamageCharacteristics(value));
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
