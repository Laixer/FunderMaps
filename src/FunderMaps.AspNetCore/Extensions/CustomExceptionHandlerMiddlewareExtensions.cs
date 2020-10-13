using FunderMaps.AspNetCore.ErrorMessaging;
using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

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
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseFunderMapsExceptionHandler(this IApplicationBuilder builder, string errorControllerPath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

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
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        internal static IApplicationBuilder UseCustomExceptionHandler<TException>(this IApplicationBuilder builder, CustomExceptionHandlerOptions options)
            where TException : Exception
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<CustomExceptionHandlerMiddleware<TException>>(Options.Create(options));
        }

        /// <summary>
        ///     Add <see cref="CustomExceptionHandlerMiddleware{FunderMapsCoreException}"/> to the builder.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="options"><see cref="CustomExceptionHandlerOptions"/></param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseFunderMapsExceptionHandler(this IApplicationBuilder builder, Action<CustomExceptionHandlerOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new CustomExceptionHandlerOptions();
            configureOptions(options);
            return builder.UseCustomExceptionHandler<FunderMapsCoreException>(options);
        }
    }
}
