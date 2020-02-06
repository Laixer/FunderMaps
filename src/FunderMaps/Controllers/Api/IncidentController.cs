using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    [Route("api/incident")]
    [ApiController]
    public class IncidentController : BaseApiController
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IAddressService _addressService;

        public IncidentController(IIncidentRepository incidentRepository, IAddressService addressService)
        {
            _incidentRepository = incidentRepository;
            _addressService = addressService;
        }

        // POST: api/incident
        [HttpPost]
        public async Task PostAsync([FromBody] IncidentInputViewModel input)
        {
            input.Address = await _addressService.GetOrCreateAddressAsync(new Address
            {
                StreetName = input.Address.StreetName,
                BuildingNumber = input.Address.BuildingNumber,
                Bag = input.Address.Bag,
            });

            await _incidentRepository.SaveIncidentAsync(input);
        }

        // NOTE: Should use authentication
        // GET: api/incident/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetAsync()
        {
            var csvFile = await _incidentRepository.ListAllIncidentsAsync();

            return File(System.Text.Encoding.UTF8.GetBytes(csvFile), "text/csv", "incident.csv");
        }
    }
}
