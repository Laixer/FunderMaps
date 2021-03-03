using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating a system process was unable to start.
    /// </summary>
    public sealed class ProcessException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Application was unable to process the request.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ProcessException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        public ProcessException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        /// <param name="innerException"><see cref="Exception.InnerException"/></param>
        public ProcessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
