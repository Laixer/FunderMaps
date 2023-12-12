using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

// FUTURE: Revamp this service.
/// <summary>
///     Service to the incidents.
/// </summary>
internal class FeedbackService(
    GeocoderTranslation geocoderTranslation,
    IEmailService emailService,
    IBlobStorageService blobStorageService,
    ILogger<FeedbackService> logger)
{
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

    public static string ToBoolean(bool? value)
        => value switch
        {
            true => "Ja",
            false => "Nee",
            _ => "Onbekend"
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
            FoundationDamageCause.JapanseKnotweed => "Japanse duizendknoop",
            FoundationDamageCause.GroundwaterLevelReduction => "Grondwaterstandverlaging",
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

    public static string ArrayToFoundationDamageCharacteristics(IEnumerable<FoundationDamageCharacteristics>? values)
    {
        if (values is not null && values.Any())
        {
            return string.Join(", ", values.Select(value => ToFoundationDamageCharacteristics(value)));
        }
        else
        {
            return "Onbekend";
        }
    }

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

    public static string ArrayToEnvironmentDamageCharacteristics(IEnumerable<EnvironmentDamageCharacteristics>? values)
    {
        if (values is not null && values.Any())
        {
            return string.Join(", ", values.Select(value => ToEnvironmentDamageCharacteristics(value)));
        }
        else
        {
            return "Onbekend";
        }
    }

    // FUTURE: split logic, hard to read.
    /// <summary>
    ///     Register a new incident.
    /// </summary>
    /// <param name="incident">Incident to process.</param>
    /// <param name="meta">Optional metadata.</param>
    public async Task<Incident> AddAsync(Incident incident, object? meta = null)
    {
        var address = await geocoderTranslation.GetAddressIdAsync(incident.Address);

        incident.Meta = meta;

        // incident.Id = await _incidentRepository.AddAsync(incident);
        // incident = await _incidentRepository.GetByIdAsync(incident.Id);

        var documentLinkList = new List<string>();
        if (incident.DocumentFile is not null)
        {
            foreach (var file in incident.DocumentFile)
            {
                Uri link = await blobStorageService.GetAccessLinkAsync(
                    containerName: Core.Constants.IncidentStorageFolderName,
                    fileName: file,
                    hoursValid: 24 * 7 * 4);

                documentLinkList.Add(link.ToString());
            }
        }

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = new[]
            {
                new EmailAddress(incident.Email, incident.Name)
            },
            Subject = $"Nieuwe melding: {incident.Id}",
            Template = "incident-customer",
            Varaibles = new Dictionary<string, object>
            {
                { "id", incident.Id },
                { "name", incident.Name ?? throw new ArgumentNullException(nameof(incident.Name)) },
                { "phone", incident.PhoneNumber ?? "-" },
                { "email", incident.Email },
                { "address", address.FullAddress },
                { "note", incident.Note ?? "-" },
                { "foundationType", ToFoundationType(incident.FoundationType) },
                { "chainedBuilding", ToBoolean(incident.ChainedBuilding) },
                { "owner", ToBoolean(incident.Owner) },
                { "neighborRecovery", ToBoolean(incident.NeighborRecovery) },
                { "foundationDamageCause", ToFoundationDamageCause(incident.FoundationDamageCause) },
                { "foundationDamageCharacteristics", ArrayToFoundationDamageCharacteristics(incident.FoundationDamageCharacteristics) },
                { "environmentDamageCharacteristics", ArrayToEnvironmentDamageCharacteristics(incident.EnvironmentDamageCharacteristics) },
            }
        });

        // foreach (var recipient in _options.Recipients)
        // {
        //     await _emailService.SendAsync(new EmailMessage
        //     {
        //         ToAddresses = new[] { new EmailAddress(recipient) },
        //         Subject = $"Nieuwe melding: {incident.Id}",
        //         Template = "incident-reviewer",
        //         Varaibles = new Dictionary<string, object>
        //         {
        //             { "id", incident.Id },
        //             { "name", incident.Name ?? throw new ArgumentNullException(nameof(incident.Name)) },
        //             { "phone", incident.PhoneNumber ?? "-" },
        //             { "email", incident.Email },
        //             { "address", address.FullAddress },
        //             { "note", incident.Note ?? "-" },
        //             { "foundationType", ToFoundationType(incident.FoundationType) },
        //             { "chainedBuilding", ToBoolean(incident.ChainedBuilding) },
        //             { "owner", ToBoolean(incident.Owner) },
        //             { "neighborRecovery", ToBoolean(incident.NeighborRecovery) },
        //             { "foundationDamageCause", ToFoundationDamageCause(incident.FoundationDamageCause) },
        //             { "foundationDamageCharacteristics", ArrayToFoundationDamageCharacteristics(incident.FoundationDamageCharacteristics) },
        //             { "environmentDamageCharacteristics", ArrayToEnvironmentDamageCharacteristics(incident.EnvironmentDamageCharacteristics) },
        //             { "documentLinks", documentLinkList },
        //         }
        //     });
        // }

        logger.LogInformation("Incident {Id} was registered", incident.Id);

        return incident;
    }
}
