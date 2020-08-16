using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Controllers
{
    /// <summary>
    ///     Base controller for all API controllers.
    /// </summary>
    [Controller]
    public abstract class BaseApiController : ControllerBase
    {
        // FUTURE: Inject User, Organization, OrganizationRole properties.
    }
}