using FunderMaps.Core.Email;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunderMaps.MapBundle;

public class ProductExporter : IHostedService
{
    private readonly HealthCheckService _healthCheckService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IEmailService _emailService;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<ProductExporter> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public ProductExporter(
        HealthCheckService healthCheckService,
        IServiceScopeFactory serviceScopeFactory,
        IBlobStorageService blobStorageService,
        IEmailService emailService,
        IHostApplicationLifetime hostApplicationLifetime,
        ILogger<ProductExporter> logger)
    {
        _healthCheckService = healthCheckService ?? throw new ArgumentNullException(nameof(healthCheckService));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);
            if (healthReport.Status != HealthStatus.Healthy)
            {
                _logger.LogError("Health check failed, stopping application");

                throw new InvalidOperationException("Health check failed, stopping application");
            }
            else
            {
                using var scope = _serviceScopeFactory.CreateScope();

                await RunAllEnabledAsync(scope, cancellationToken);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error while creating product export");

            await _emailService.SendAsync(new EmailMessage
            {
                Subject = "Error while creating product export",
                Content = "Error while creating product export, see log for details.",
            });
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }

    /// <summary>
    ///     Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task RunAllEnabledAsync(IServiceScope scope, CancellationToken cancellationToken)
    {
        var telemetryRepository = scope.ServiceProvider.GetRequiredService<ITelemetryRepository>();

        await foreach (var organization in telemetryRepository.ListLastMonthOrganizationaAsync())
        {
            try
            {
                string filePath = $"product_tracker_{organization}.csv";

                var currentDate = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

                string lastMonthName = lastDayOfLastMonth.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);

                var csvConfig = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true,
                };

                using var writer = new StreamWriter(filePath);
                using var csv = new CsvHelper.CsvWriter(writer, csvConfig);

                await csv.WriteRecordsAsync(telemetryRepository.ListLastMonthByOrganizationIdAsync(organization));

                await _blobStorageService.StoreFileAsync($"product/export_{lastMonthName.ToLower()}_{organization}.csv", filePath);
            }
            finally
            {
                var currentDirectory = Directory.GetCurrentDirectory();

                FileHelper.DeleteFilesWithExtension(currentDirectory, "csv");
            }
        }
    }
}
