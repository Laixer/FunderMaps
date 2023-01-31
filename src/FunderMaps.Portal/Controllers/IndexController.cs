using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Portal.Controllers;

public class IndexController : Controller
{
    public ActionResult Index()
    {
        return RedirectPermanent("https://incident.fundermaps.com/");
    }
}
