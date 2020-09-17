using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating we can't find or parse a FunderMaps product.
    /// </summary>
    public sealed class ProductNotFoundException : FunderMapsCoreException
    {
        /// <summary>
        ///     Indicates we can't find this product.
        /// </summary>
        public ProductNotFoundException()
        {
        }

        /// <summary>
        ///     Indicates we can't find this product.
        /// </summary>
        /// <param name="message">Message to display</param>
        public ProductNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Indicates we can't find this product.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="innerException"><see cref="Exception"/></param>
        public ProductNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
