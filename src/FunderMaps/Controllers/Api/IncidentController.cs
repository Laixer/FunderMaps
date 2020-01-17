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

        // POST: api/Incident
        [HttpPost]
        public async Task Post([FromBody] IncidentInputViewModel input)
        {
            input.Address = await _addressService.GetOrCreateAddressAsync(new Address
            {
                StreetName = input.Address.StreetName,
                BuildingNumber = (short)input.Address.BuildingNumber,
                Bag = input.Address.Bag,
            });

            await _incidentRepository.SaveIncidentAsync(input);
        }
    }
}
