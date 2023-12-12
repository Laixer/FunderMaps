using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

public abstract class FunderMapsController : ControllerBase
{
    /// <summary>
    ///    Get the user id from the current session.
    /// </summary>
    public Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

    /// <summary>
    ///   Get the tenant id from the current session.
    /// </summary>
    public Guid TenantId => Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());
}
