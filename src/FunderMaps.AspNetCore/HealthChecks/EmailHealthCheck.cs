using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FunderMaps.AspNetCore.HealthChecks
{
    /// <summary>
    ///     Check if the email backend is alive.
    /// </summary>
    public class EmailHealthCheck : IHealthCheck
    {
        private readonly IEmailService _emailService;

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        public EmailHealthCheck(IEmailService emailService)
            => _emailService = emailService;

        /// <summary>
        ///     Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns>Instance of <see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            // Operation throws if an error occurs.
            await _emailService.TestService();
            return HealthCheckResult.Healthy();
        }
    }
}
