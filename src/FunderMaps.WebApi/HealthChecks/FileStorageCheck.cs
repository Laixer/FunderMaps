﻿using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace FunderMaps.HealthChecks
{
    /// <summary>
    ///     Check if the file storage service is working.
    /// </summary>
    public class FileStorageCheck : IHealthCheck
    {
        private readonly IBlobStorageService _blobStorageService;

        /// <summary>
        ///     Create a new instance.
        /// </summary>
        /// <param name="blobStorageService">File storage service.</param>
        public FileStorageCheck(IBlobStorageService blobStorageService)
            => _blobStorageService = blobStorageService;

        /// <summary>
        ///     Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A System.Threading.CancellationToken that can be used to cancel the health check.</param>
        /// <returns><see cref="HealthCheckResult"/>.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return HealthCheckResult.Unhealthy();

            //return (await _fileStorageService.GetStorageNameAsync()).ToLowerInvariant().Contains(Constants.ReportStorage)
            //               ? HealthCheckResult.Healthy()
            //               : HealthCheckResult.Unhealthy();
        }
    }
}

