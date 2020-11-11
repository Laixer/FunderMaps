using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Core;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Infrastructure.BatchClient
{
    internal class ChannelFactory : IDisposable
    {
        private readonly GrpcChannel rpcChannel;
        private readonly BatchOptions _options;

        private bool disposedValue;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ChannelFactory(IOptions<BatchOptions> options, ILoggerFactory loggerFactory)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            // NOTE: The httpHandler is disposed when the channel is disposed.
            HttpClientHandler httpHandler = new HttpClientHandler();

            if (!_options.TlsValidate)
            {
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            rpcChannel = GrpcChannel.ForAddress(_options.ServiceUri, new GrpcChannelOptions
            {
                HttpHandler = httpHandler,
                DisposeHttpClient = true,
                LoggerFactory = loggerFactory,
            });
        }

        /// <summary>
        ///     Get a remote channel to the GRPC service.
        /// </summary>
        public ChannelBase RemoteChannel => rpcChannel;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    rpcChannel.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
