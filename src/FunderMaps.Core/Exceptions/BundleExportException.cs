using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicates some error occurred during bundle export.
    /// </summary>
    public sealed class BundleExportException : BackgroundWorkBaseException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleExportException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleExportException(string message) 
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleExportException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleExportException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
