using System;
using Microsoft.Extensions.Logging;
using Npgsql.Logging;

namespace FunderMaps.Data
{
    public class DataNpgsqlLogger : NpgsqlLogger
    {
        private readonly ILogger<DataNpgsqlLogger> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public DataNpgsqlLogger(ILogger<DataNpgsqlLogger> logger)
            => _logger = logger;

        public override bool IsEnabled(NpgsqlLogLevel level)
            => _logger.IsEnabled(MapLogLevel(level));

        public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception exception = null)
        {
            _logger.Log(MapLogLevel(level), exception, $"{connectorId} : {msg}");
        }

        private LogLevel MapLogLevel(NpgsqlLogLevel logLevel)
            => logLevel switch
            {
                NpgsqlLogLevel.Debug => LogLevel.Debug,
                NpgsqlLogLevel.Error => LogLevel.Error,
                NpgsqlLogLevel.Fatal => LogLevel.Critical,
                NpgsqlLogLevel.Info => LogLevel.Information,
                NpgsqlLogLevel.Trace => LogLevel.Trace,
                NpgsqlLogLevel.Warn => LogLevel.Warning,
                _ => LogLevel.None,
            };
    }
}
