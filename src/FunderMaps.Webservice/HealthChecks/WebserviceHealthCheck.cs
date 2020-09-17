using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.HealthChecks
{
    /// <summary>
    /// Implements health checks for the webservice.
    /// </summary>
    public sealed class WebserviceHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
