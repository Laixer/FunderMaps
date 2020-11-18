using FunderMaps.AspNetCore.ErrorMessaging;
using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FunderMaps.AspNetCore.Extensions
{
    /// <summary>
    ///     Extension functionality for <see cref="CustomExceptionHandlerMiddleware{TException}"/>.
    /// </summary>
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        ///     Add <see cref="CustomExceptionHandlerMiddleware{FunderMapsCoreException}"/> to the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="errorControllerPath">Path which leads to error handler.</param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseFunderMapsExceptionHandler([DisallowNull] this IApplicationBuilder builder, string errorControllerPath)
        {
            return builder.UseCustomExceptionHandler<FunderMapsCoreException>(new CustomExceptionHandlerOptions()
            {
                ErrorControllerPath = errorControllerPath
            });
        }

        /// <summary>
        ///     Add <see cref="CustomExceptionHandlerMiddleware{TException}"/> to the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="options"><see cref="CustomExceptionHandlerOptions"/></param>
        /// <typeparam name="TException">Exception base class to catch.</typeparam>
        /// <returns>Instance of <see cref="IApplicationBuilder"/>.</returns>
        internal static IApplicationBuilder UseCustomExceptionHandler<TException>([DisallowNull] this IApplicationBuilder builder, CustomExceptionHandlerOptions options)
            where TException : Exception
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<CustomExceptionHandlerMiddleware<TException>>(Options.Create(options));
        }
    }
}
