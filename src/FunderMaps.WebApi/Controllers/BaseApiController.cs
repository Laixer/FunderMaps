using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers
{
    // TODO: Remove
    /// <summary>
    ///     Base controller for all API controllers.
    /// </summary>
    [Controller]
    public abstract class BaseApiController : ControllerBase
    {
        // FUTURE: Inject User, Organization, OrganizationRole properties.
    }
}