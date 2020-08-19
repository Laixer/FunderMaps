using FunderMaps.AspNetCore.ErrorMessaging;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using System.Net;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Maps a <see cref="FunderMapsCoreException"/> to an <see cref="ErrorMessageFeature"/>.
    /// </summary>
    public sealed class FunderMapsExceptionMapper : IExceptionMapper<FunderMapsCoreException>
    {
        /// <summary>
        ///     Maps a <see cref="FunderMapsCoreException"/> to the 
        ///     corresponding <see cref="ErrorMessageFeature"/>.
        /// </summary>
        /// <param name="exception"><see cref="FunderMapsCoreException"/></param>
        /// <returns><see cref="ErrorMessageFeature"/></returns>
        public IErrorMessage Map(FunderMapsCoreException exception)
            => exception switch
            {
                ProductNotFoundException x => BuildMessage("Could not find product", HttpStatusCode.Conflict),
                _ => BuildMessage("Unable to process request", HttpStatusCode.InternalServerError)
            };

        /// <summary>
        ///     Builds an <see cref="ErrorMessageFeature"/>.
        /// </summary>
        /// <param name="message">Message to display.</param>
        /// <param name="statusCode">Status code to display.</param>
        /// <returns></returns>
        private ErrorMessageFeature BuildMessage(string message, HttpStatusCode statusCode)
            => new ErrorMessageFeature
            {
                Message = message,
                StatusCode = (int)statusCode
            };
    }
}
