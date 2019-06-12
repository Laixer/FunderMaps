using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FunderMaps.Middleware
{
    /// <summary>
    /// An ASP.NET Core middleware for additional security.
    /// </summary>
    public sealed class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityPolicy _policy;

        /// <summary>
        /// Instantiates a new <see cref="SecurityMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="policy">An instance of the <see cref="SecurityPolicy"/> which can be applied.</param>
        public SecurityMiddleware(RequestDelegate next, SecurityPolicy policy)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.Response;
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var headers = response.Headers;

            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers[headerValuePair.Key] = headerValuePair.Value;
            }

            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }

            await _next(context);
        }
    }
}
