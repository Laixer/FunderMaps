using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using System.Net;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Maps an <see cref="FunderMapsCoreException"/> to an <see cref="ErrorMessage"/>.
    /// </summary>
    public class FunderMapsExceptionMapper : IExceptionMapper<FunderMapsCoreException>
    {
        /// <summary>
        ///     Builds an <see cref="ErrorMessage"/>.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="statusCode">Status code to display.</param>
        /// <returns>Instance of<see cref="ErrorMessage"/>.</returns>
        protected static ErrorMessage BuildMessage(string message, HttpStatusCode statusCode)
            => new()
            {
                Message = message,
                StatusCode = (int)statusCode
            };

        /// <summary>
        ///     Maps a <see cref="FunderMapsCoreException"/> to an <see cref="ErrorMessage"/>.
        /// </summary>
        /// <param name="exception">An instance of <see cref="FunderMapsCoreException"/>.</param>
        /// <returns>Instance of<see cref="ErrorMessage"/>.</returns>
        public ErrorMessage Map(FunderMapsCoreException exception)
            => exception switch
            {
                AuthenticationException _ => BuildMessage(exception.Message, HttpStatusCode.Unauthorized),
                AuthorizationException _ => BuildMessage(exception.Message, HttpStatusCode.Forbidden),
                EntityNotFoundException _ => BuildMessage(exception.Message, HttpStatusCode.NotFound),
                EntityReadOnlyException _ => BuildMessage(exception.Message, HttpStatusCode.Locked),
                InvalidCredentialException _ => BuildMessage(exception.Message, HttpStatusCode.Forbidden),
                InvalidIdentifierException _ => BuildMessage(exception.Message, HttpStatusCode.BadRequest),
                OperationAbortedException _ => BuildMessage(exception.Message, HttpStatusCode.BadRequest),
                ProcessException _ => BuildMessage(exception.Message, HttpStatusCode.InternalServerError),
                QueueOverflowException _ => BuildMessage(exception.Message, HttpStatusCode.InternalServerError),
                ReferenceNotFoundException _ => BuildMessage(exception.Message, HttpStatusCode.NotFound),
                ServiceUnavailableException _ => BuildMessage(exception.Message, HttpStatusCode.ServiceUnavailable),
                StateTransitionException _ => BuildMessage(exception.Message, HttpStatusCode.NotAcceptable),
                StorageException _ => BuildMessage(exception.Message, HttpStatusCode.InternalServerError),
                UnhandledTaskException _ => BuildMessage(exception.Message, HttpStatusCode.InternalServerError),
                _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError)
            };
    }
}
