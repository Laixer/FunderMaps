using FunderMaps.AspNetCore.ErrorMessaging;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.AspNetCore.Extensions
{
    /// <summary>
    ///     Extension functionality to add exception mapping to the services.
    /// </summary>
    public static class ExceptionMapperServiceCollectionExtensions
    {
        /// <summary>
        ///     Add <see cref="FunderMapsExceptionMapper"/> to the services.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddFunderMapsExceptionMapper(this IServiceCollection services) 
            => services.AddSingleton<IExceptionMapper<FunderMapsCoreException>, FunderMapsExceptionMapper>();
    }
}
