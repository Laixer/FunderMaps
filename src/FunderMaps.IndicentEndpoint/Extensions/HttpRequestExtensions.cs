using Microsoft.AspNetCore.Http;
using System;

namespace FunderMaps.IndicentEndpoint.Extensions
{
    /// <summary>
    ///     HttpRequest extensions.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        ///     Get user agent from client.
        /// </summary>
        /// <param name="request">Instance to extend.</param>
        /// <returns>String containing the user agent or null.</returns>
        public static string UserAgent(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers["User-Agent"].ToString();
        }
    }
}
