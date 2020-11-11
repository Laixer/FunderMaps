using System;

namespace FunderMaps.Infrastructure.BatchClient
{
    /// <summary>
    ///     Options for the batch service.
    /// </summary>
    public sealed class BatchOptions
    {
        /// <summary>
        ///     Base service uri for batch service.
        /// </summary>
        public Uri ServiceUri { get; set; }

        /// <summary>
        ///     Name of the blob storage.
        /// </summary>
        public bool TlsValidate { get; set; }
    }
}
