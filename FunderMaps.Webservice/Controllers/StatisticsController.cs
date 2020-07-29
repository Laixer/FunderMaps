using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Webservice.Controllers
{
    public class StatisticsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}