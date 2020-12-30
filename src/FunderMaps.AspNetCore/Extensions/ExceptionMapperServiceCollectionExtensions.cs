using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FunderMaps.AspNetCore.Extensions
{
    /// <summary>
    ///     Extension functionality to add exception mapping to the services.
    /// </summary>
    internal static class ExceptionMapperServiceCollectionExtensions
    {
        /// <summary>
        ///     Add <see cref="FunderMapsExceptionMapper"/> to the services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddFunderMapsExceptionMapper(this IServiceCollection services)
            => services.AddSingleton<IExceptionMapper<FunderMapsCoreException>, FunderMapsExceptionMapper>();
    }
}
