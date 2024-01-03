using FunderMaps.Core.Entities;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

// FUTURE: Revamp this service.
/// <summary>
///     Service to the incidents.
/// </summary>
public class FeedbackService(ILogger<FeedbackService> logger)
{
    // FUTURE: split logic, hard to read.
    /// <summary>
    ///     Register a new incident.
    /// </summary>
    /// <param name="incident">Incident to process.</param>
    /// <param name="meta">Optional metadata.</param>
    public async Task<Incident> AddAsync(Incident incident, object? meta = null)
    {
        // var address = await geocoderTranslation.GetAddressIdAsync(incident.Address);

        // incident.Meta = meta;

        // incident.Id = await _incidentRepository.AddAsync(incident);
        // incident = await _incidentRepository.GetByIdAsync(incident.Id);

        // var documentLinkList = new List<string>();
        // if (incident.DocumentFile is not null)
        // {
        //     foreach (var file in incident.DocumentFile)
        //     {
        //         Uri link = await blobStorageService.GetAccessLinkAsync(
        //             containerName: Core.Constants.IncidentStorageFolderName,
        //             fileName: file,
        //             hoursValid: 24 * 7 * 4);

        //         documentLinkList.Add(link.ToString());
        //     }
        // }

        // await emailService.SendAsync(new EmailMessage
        // {
        //     ToAddresses = new[]
        //     {
        //         new EmailAddress(incident.Email, incident.Name)
        //     },
        //     Subject = $"Nieuwe melding: {incident.Id}",
        //     Template = "incident-customer",
        //     Varaibles = new Dictionary<string, object>
        //     {
        //         { "id", incident.Id },
        //         { "name", incident.Name ?? throw new ArgumentNullException(nameof(incident.Name)) },
        //         { "phone", incident.PhoneNumber ?? "-" },
        //         { "email", incident.Email },
        //         { "address", address.FullAddress },
        //         { "note", incident.Note ?? "-" },
        //         { "foundationType", ToFoundationType(incident.FoundationType) },
        //         { "chainedBuilding", ToBoolean(incident.ChainedBuilding) },
        //         { "owner", ToBoolean(incident.Owner) },
        //         { "neighborRecovery", ToBoolean(incident.NeighborRecovery) },
        //         { "foundationDamageCause", ToFoundationDamageCause(incident.FoundationDamageCause) },
        //         { "foundationDamageCharacteristics", ArrayToFoundationDamageCharacteristics(incident.FoundationDamageCharacteristics) },
        //         { "environmentDamageCharacteristics", ArrayToEnvironmentDamageCharacteristics(incident.EnvironmentDamageCharacteristics) },
        //     }
        // });

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

        logger.LogInformation("Feedback was submitted but ignored.");

        await Task.CompletedTask;

        return incident;
    }
}
