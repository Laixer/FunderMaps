using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using System;
using System.Net;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Base exception mapper.
    /// </summary>
    /// <typeparam name="TException"><see cref="Exception"/></typeparam>
    public abstract class ExceptionMapperBase<TException> : IExceptionMapper<TException>
        where TException : Exception
    {
        /// <summary>
        ///     Maps an <see cref="Exception"/> to the a default
        ///     <see cref="ErrorMessage"/>.
        /// </summary>
        /// <remarks>
        ///     This displays the exception message. If this is 
        ///     undesired behaviour, override <see cref="Map(TException)"/>.
        /// </remarks>
        /// <param name="exception"><see cref="Exception"/></param>
        /// <returns><see cref="ErrorMessage"/></returns>
        public virtual ErrorMessage Map(TException exception)
            => BuildMessage(exception?.Message, HttpStatusCode.InternalServerError);

        /// <summary>
        ///     Builds an <see cref="ErrorMessage"/>.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="statusCode">Status code to display.</param>
        /// <returns></returns>
        protected virtual ErrorMessage BuildMessage(string message, HttpStatusCode statusCode)
            => new ErrorMessage
            {
                Message = message,
                StatusCode = (int)statusCode
            };
    }
}
