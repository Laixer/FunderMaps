using FunderMaps.Core.Email;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace FunderMaps.AspNetCore.HealthChecks;

/// <summary>
///     Check if the email backend is alive.
/// </summary>
public class EmailHealthCheck : IHealthCheck
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailHealthCheck> _logger;

    /// <summary>
    ///     Create a new instance.
    /// </summary>
    public EmailHealthCheck(IEmailService emailService, ILogger<EmailHealthCheck> logger)
        => (_emailService, _logger) = (emailService, logger);

    /// <summary>
    ///     Runs the health check, returning the status of the component being checked.
    /// </summary>
    /// <param name="context">A context object associated with the current execution.</param>
    /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
    /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        try
        {
            await _emailService.HealthCheck();
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            _logger.LogTrace(exception, "Health check failed");

            return HealthCheckResult.Unhealthy("email service");
        }
    }
}
