﻿using System;

namespace FunderMaps.Core.Exceptions
{
    /// <summary>
    ///     Exception indicating a product request was invalid.
    /// </summary>
    public sealed class InvalidProductRequestException : FunderMapsCoreException
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public InvalidProductRequestException()
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        public InvalidProductRequestException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="message"><see cref="Exception.Message"/></param>
        /// <param name="innerException"><see cref="Exception.InnerException"/></param>
        public InvalidProductRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        s
    }
}
