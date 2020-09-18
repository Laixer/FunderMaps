using System;

namespace FunderMaps.Core.Exceptions
{
    // TODO: rename to FileUploadException
    /// <summary>
    ///     Indicates an error occured during or after upload.
    /// </summary>
    public class UploadException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UploadException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UploadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UploadException()
        {
        }
    }
}
