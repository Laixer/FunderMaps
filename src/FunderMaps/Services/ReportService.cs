using FunderMaps.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunderMaps.Services
{
    /// <summary>
    /// Report service.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="logger"></param>
        public ReportService(ILogger<ReportService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Create new report.
        /// </summary>
        public Task CreateAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Delete report.
        /// </summary>
        public Task DeleteAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Validate report.
        /// </summary>
        public Task ValidateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
