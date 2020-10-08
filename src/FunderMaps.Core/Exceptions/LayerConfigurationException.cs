using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Indicates our bundle layer configuration is incorrect.
    /// </summary>
    public sealed class LayerConfigurationException : FunderMapsCoreException
    {
        public LayerConfigurationException()
        {
        }

        public LayerConfigurationException(string message) : base(message)
        {
        }

        public LayerConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LayerConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
