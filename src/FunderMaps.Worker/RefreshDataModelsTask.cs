using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Worker;

public class RefreshDataModelsTask : ISingleShotTask
{
    private readonly IEmailService _emailService;
    private readonly ITelemetryRepository _telemetryRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public RefreshDataModelsTask(
        IEmailService emailService,
        IBlobStorageService blobStorageService,
        ITelemetryRepository telemetryRepository,
        ILogger<RefreshDataModelsTask> logger)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _telemetryRepository = telemetryRepository ?? throw new ArgumentNullException(nameof(telemetryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
