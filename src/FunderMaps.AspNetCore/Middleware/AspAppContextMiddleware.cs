using Microsoft.AspNetCore.Http;

namespace FunderMaps.AspNetCore.Middleware;

/// <summary>
///     Create the <see cref="Core.AppContext"/> from the <see cref="HttpContext"/>.
/// </summary>
public class AspAppContextMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public AspAppContextMiddleware(RequestDelegate next) => _next = next;

    /// <summary>
    ///     Invoke this middleware.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
    /// <param name="appContext">The <see cref="AppContext"/>.</param>
    public async Task InvokeAsync(HttpContext httpContext, Core.AppContext appContext)
    {
        appContext.CancellationToken = httpContext.RequestAborted;

        await _next(httpContext);
    }
}
