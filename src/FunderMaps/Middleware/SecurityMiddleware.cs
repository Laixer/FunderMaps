using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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

        /// <summary>
        /// Add additional HTTP headers to the response.
        /// </summary>
        /// <param name="headers">Header dictionary.</param>
        private void HttpHeadersAdd(IHeaderDictionary headers)
        {
            foreach (var headerValuePair in _policy.SetHeaders)
            {
                headers.Add(headerValuePair.Key, headerValuePair.Value);
            }
        }

        /// <summary>
        /// Remove HTTP headers from the response.
        /// </summary>
        /// <param name="headers">Header dictionary.</param>
        private void HttpHeadersRemove(IHeaderDictionary headers)
        {
            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }
        }

        /// <summary>
        /// Invoke middleware.
        /// </summary>
        /// <param name="context">Http context.</param>
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

            HttpHeadersAdd(response.Headers);
            HttpHeadersRemove(response.Headers);

            await _next(context);
        }
    }
}
