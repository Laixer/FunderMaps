using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
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

        // if (httpContext.User.Identity is not null && httpContext.User.Identity.IsAuthenticated)
        // {
        // foreach (var orgClaim in httpContext.User.FindAll("organization_id"))
        // {
        //     appContext.Organizations.Add(new()
        //     {
        //         Id = Guid.Parse(orgClaim.Value),
        //     });
        // }

        // var tenantClaim = httpContext.User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
        // if (tenantClaim is not null)
        // {
        //     appContext.Organizations.Add(new()
        //     {
        //         Id = Guid.Parse(tenantClaim.Value),
        //     });
        // }

        // var fundermapsTenantClaim = httpContext.User.FindFirst(FunderMapsAuthenticationClaimTypes.Tenant);
        // if (fundermapsTenantClaim is not null)
        // {
        //     appContext.Organizations.Add(new()
        //     {
        //         Id = Guid.Parse(fundermapsTenantClaim.Value),
        //     });
        // }

        // Organization? preferredOrganization = null;
        // foreach (var preferredOrganizationId in httpContext.Request.Headers["PreferredOrganizationId"])
        // {
        //     if (preferredOrganizationId is not null)
        //     {
        //         preferredOrganization = appContext.Organizations.Find(x => x.Id == Guid.Parse(preferredOrganizationId));
        //     }

        //     if (preferredOrganization is not null)
        //     {
        //         appContext.ActiveOrganization = preferredOrganization;
        //         break;
        //     }
        // }

        // if (preferredOrganization is null && appContext.Organizations.Any())
        // {
        //     appContext.ActiveOrganization = appContext.Organizations.First();
        // }
        // }

        await _next(httpContext);
    }
}
