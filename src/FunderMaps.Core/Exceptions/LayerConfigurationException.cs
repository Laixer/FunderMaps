using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicates our bundle layer configuration is incorrect.
    /// </summary>
    public sealed class LayerConfigurationException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerConfigurationException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public LayerConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
