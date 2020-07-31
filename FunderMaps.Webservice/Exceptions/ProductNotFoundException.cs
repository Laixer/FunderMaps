using System;
using System.Runtime.Serialization;

namespace FunderMaps.Webservice.Exceptions
{
    /// <summary>
    /// Exception indicating we can't find or parse a FunderMaps product.
    /// </summary>
    public sealed class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string message) : base(message)
        {
        }

        public ProductNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
