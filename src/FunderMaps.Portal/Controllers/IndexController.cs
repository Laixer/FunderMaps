using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Portal.Controllers;

public class IndexController : Controller
{
    public ActionResult Index()
    {
        if (Request.Host.Host.ToLower().Contains("schiedam"))
        {
            return RedirectPermanent("https://incident.fundermaps.com/?vendor=schiedam");
        }
        else if (Request.Host.Host.ToLower().Contains("lansingerland"))
        {
            return RedirectPermanent("https://incident.fundermaps.com/?vendor=lansingerland");
        }
        else if (Request.Host.Host.ToLower().Contains("regiodeal"))
        {
            return RedirectPermanent("https://incident.fundermaps.com/?vendor=regiodeal");
        }
        else if (Request.Host.Host.ToLower().Contains("veenweidefryslan"))
        {
            return RedirectPermanent("https://incident.fundermaps.com/?vendor=veenweidefryslan");
        }

        return RedirectPermanent("https://incident.fundermaps.com/");
    }
}
