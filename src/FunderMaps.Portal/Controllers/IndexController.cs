using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Portal.Controllers;

// FUTURE: Split the logic into two separate controllers.
/// <summary>
///     Endpoint controller for incident operations.
/// </summary>
// [AllowAnonymous]
// [Route("index")]
public class IndexController : Controller
{
    public ActionResult Index()
    {
        return Redirect("https://incident.fundermaps.com/");
    }
}
