using System;
using System.Threading.Tasks;
using FunderMaps.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Services
{
    // FUTURE: Can be removed?
    public class ReportService : IReportService
    {
        private readonly ILogger _logger;

        public const string FileContainer = "report";

        public ReportService(ILogger<ReportService> logger)
        {
            _logger = logger;
        }

        public Task CreateAsync()
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync()
        {
            return Task.CompletedTask;
        }

        public Task ValidateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
