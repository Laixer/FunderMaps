using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Core;
using System;

namespace FunderMaps.Infrastructure.BatchClient
{
    internal class ChannelFactory : IDisposable
    {
        private HttpClientHandler httpHandler = new HttpClientHandler();

        private bool disposedValue;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ChannelFactory()
        {
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        }

        /// <summary>
        ///     Get a remote channel to the GRPC service.
        /// </summary>
        public ChannelBase RemoteChannel
        {
            get
            {
                // The port number(5001) must match the port of the gRPC server.
                return GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    httpHandler.Dispose();
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
