using FunderMaps.Core.Extensions;
using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Default exception for the core assembly.
    /// </summary>
    /// <remarks>
    ///     All exception in this assembly ought to inherit from this
    ///     exception
    /// </remarks>
    public abstract class FunderMapsCoreException : Exception
    {
        /// <summary>
        ///     Exception title
        /// </summary>
        public virtual string Title => "Application was unable to process the request.";

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FunderMapsCoreException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FunderMapsCoreException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public FunderMapsCoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        protected FunderMapsCoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Deconstruct this exception into a title and a message.
        /// </summary>
        /// <remarks>
        ///     The returned message value will have the messages of all the inner exceptions joined.
        /// </remarks>
        public void Deconstruct(out string title, out string message)
        {
            title = Title;
            message = this.GetMessageWithInner();
        }
    }
}
