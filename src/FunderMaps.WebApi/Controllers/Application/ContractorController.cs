using FunderMaps.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application
{
    [Authorize]
    [ApiController, Route("api/contractor")]
    public class ContractorController : BaseApiController
    {
    }
}
