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
                AuthenticationException _ => BuildMessage("Login attempt failed with provided credentials.", HttpStatusCode.Unauthorized),
                AuthorizationException _ => BuildMessage("Access to resource forbidden.", HttpStatusCode.Forbidden),
                EntityNotFoundException _ => BuildMessage("Requested entity not found.", HttpStatusCode.NotFound),
                EntityReadOnlyException _ => BuildMessage("Requested entity is immutable.", HttpStatusCode.Locked),
                InvalidCredentialException _ => BuildMessage("Action failed with provided credentials.", HttpStatusCode.Forbidden),
                InvalidIdentifierException _ => BuildMessage("Action failed with provided identifier.", HttpStatusCode.BadRequest),
                OperationAbortedException _ => BuildMessage("Operation was aborted by client.", HttpStatusCode.BadRequest),
                ProcessException _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError),
                QueueOverflowException _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError),
                ReferenceNotFoundException _ => BuildMessage("Referenced entity not found.", HttpStatusCode.NotFound),
                ServiceUnavailableException _ => BuildMessage("Internal service was unable to process the request.", HttpStatusCode.ServiceUnavailable),
                StateTransitionException _ => BuildMessage("Requested entity cannot change state.", HttpStatusCode.NotAcceptable),
                StorageException _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError),
                UnhandledTaskException _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError),
                _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError)
            };
    }
}
