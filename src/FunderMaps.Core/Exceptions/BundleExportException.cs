using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicates some error occurred during bundle export.
    /// </summary>
    public sealed class BundleExportException : BackgroundWorkBaseException
    {
        public BundleExportException()
        {
        }

        public BundleExportException(string message) 
            : base(message)
        {
        }

        public BundleExportException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public BundleExportException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
