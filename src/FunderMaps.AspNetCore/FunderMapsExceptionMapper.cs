using FunderMaps.AspNetCore.ErrorMessaging;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;
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
        public override IErrorMessage Map(FunderMapsCoreException exception)
            => exception switch
            {
                ProductNotFoundException x => BuildMessage("Could not find product", HttpStatusCode.Conflict),
                _ => BuildMessage("Unable to process request", HttpStatusCode.InternalServerError)
            };
    }
}
