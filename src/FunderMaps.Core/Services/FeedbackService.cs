using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Core.Services;

// FUTURE: Revamp this service.
/// <summary>
///     Service to the incidents.
/// </summary>
public class FeedbackService(
    IOptions<IncidentOptions> options,
    GeocoderTranslation geocoderTranslation,
    IEmailService emailService,
    IBlobStorageService blobStorageService,
    ILogger<IncidentService> logger)
{
    private readonly IncidentOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

    // FUTURE: split logic, hard to read.
    /// <summary>
    ///     Register a new incident.
    /// </summary>
    /// <param name="incident">Incident to process.</param>
    /// <param name="meta">Optional metadata.</param>
    public async Task<Incident> AddAsync(Incident incident, object? meta = null)
    {
        var address = await geocoderTranslation.GetAddressIdAsync(incident.Building);

        var documentLinkList = new List<string>();
        if (incident.DocumentFile is not null)
        {
            foreach (var file in incident.DocumentFile)
            {
                Uri link = await blobStorageService.GetAccessLinkAsync(
                    containerName: Constants.IncidentStorageFolderName,
                    fileName: file,
                    hoursValid: 24 * 7 * 4);

                documentLinkList.Add(link.ToString());
            }
        }

        foreach (var recipient in _options.Recipients)
        {
            await emailService.SendAsync(new EmailMessage
            {
                ToAddresses = [new EmailAddress(recipient)],
                Subject = $"Nieuwe melding: {incident.Id}",
                Template = "incident-reviewer",
                Varaibles = new Dictionary<string, object>
                {
                    { "id", incident.Id },
                    { "name", incident.Name ?? throw new ArgumentNullException(nameof(incident.Name)) },
                    { "phone", incident.PhoneNumber ?? "-" },
                    { "email", incident.Email },
                    { "address", address.FullAddress },
                    { "note", incident.Note ?? "-" },
                    { "foundationType", incident.FoundationType },
                    { "chainedBuilding", incident.ChainedBuilding },
                    { "owner", incident.Owner },
                    { "neighborRecovery", incident.NeighborRecovery },
                    { "foundationDamageCause", incident.FoundationDamageCause },
                    { "foundationDamageCharacteristics", incident.FoundationDamageCharacteristics },
                    { "environmentDamageCharacteristics", incident.EnvironmentDamageCharacteristics },
                    { "documentLinks", documentLinkList },
                }
            });
        }

        logger.LogInformation("Feedback was submitted but ignored.");

        await Task.CompletedTask;

        return incident;
    }
}
