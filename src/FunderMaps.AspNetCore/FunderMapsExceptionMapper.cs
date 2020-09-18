using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types;
using System.Net;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Maps a <see cref="FunderMapsCoreException"/> to an <see cref="ErrorMessageFeature"/>.
    /// </summary>
    public sealed class FunderMapsExceptionMapper : ExceptionMapperBase<FunderMapsCoreException>
    {
        /// <summary>
        ///     Maps a <see cref="FunderMapsCoreException"/> to the 
        ///     corresponding <see cref="ErrorMessageFeature"/>.
        /// </summary>
        /// <param name="exception"><see cref="FunderMapsCoreException"/></param>
        /// <returns><see cref="ErrorMessageFeature"/></returns>
        public override ErrorMessage Map(FunderMapsCoreException exception)
            => exception switch
            {
                AuthenticationException _ => BuildMessage("Login attempt failed with provided credentials.", HttpStatusCode.Unauthorized),
                AuthorizationException _ => BuildMessage("Access to resource forbidden.", HttpStatusCode.Forbidden),
                EntityNotFoundException _ => BuildMessage("Requested entity not found.", HttpStatusCode.NotFound),
                EntityReadOnlyException _ => BuildMessage("Requested entity is immutable.", HttpStatusCode.Locked),
                InvalidCredentialException _ => BuildMessage("Action failed with provided credentials.", HttpStatusCode.Forbidden),
                InvalidProductRequestException _ => BuildMessage("Invalid product requested.", HttpStatusCode.BadRequest),
                StateTransitionException _ => BuildMessage("Requested entity cannot change state.", HttpStatusCode.NotAcceptable),
                StorageException _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError),
                UploadException _ => BuildMessage("File upload failed.", HttpStatusCode.BadRequest),
                _ => BuildMessage("Application was unable to process the request.", HttpStatusCode.InternalServerError)
            };
    }
}
