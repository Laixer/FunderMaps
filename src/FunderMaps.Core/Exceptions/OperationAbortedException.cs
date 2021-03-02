using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating a product request was invalid.
    /// </summary>
    public sealed class OperationAbortedException : FunderMapsCoreException
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public override string Title => "Operation was aborted by client.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OperationAbortedException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        public OperationAbortedException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        /// <param name="innerException"><see cref="Exception.InnerException"/></param>
        public OperationAbortedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
