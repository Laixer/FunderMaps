using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    [Route("api/incident")]
    [ApiController]
    public class IncidentController : BaseApiController
    {
        private readonly IIncidentRepository _incidentRepository;
        private readonly IAddressService _addressService;
        private readonly IEmailService _emailService;

        public IncidentController(IIncidentRepository incidentRepository, IAddressService addressService, IEmailService emailService)
        {
            _incidentRepository = incidentRepository;
            _addressService = addressService;
            _emailService = emailService;
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

            var body = @$"Reported new incident: <br />
            <table>
                <thead>
                    <tr>
                        <th>Onderdeel</th>
                        <th>Gegeven waarden</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>address</td>
                        <td>{input.Address.StreetName}</td>
                    </tr>
                    <tr>
                        <td>foundation_type</td>
                        <td>{input.FoundationType}</td>
                    </tr>
                    <tr>
                        <td>chained_building</td>
                        <td>{input.ChainedBuilding}</td>
                    </tr>
                    <tr>
                        <td>owner</td>
                        <td>{input.Owner}</td>
                    </tr>
                    <tr>
                        <td>foundation_recovery</td>
                        <td>{input.FoundationRecovery}</td>
                    </tr>
                    <tr>
                        <td>foundation_damage_cause</td>
                        <td>{input.FoundationDamageCause}</td>
                    </tr>
                    <tr>
                        <td>contact_name</td>
                        <td>{input.Name}</td>
                    </tr>
                    <tr>
                        <td>contact_email</td>
                        <td>{input.Email}</td>
                    </tr>
                    <tr>
                        <td>contact_phonenumber</td>
                        <td>{input.Phonenumber}</td>
                    </tr>
                    <tr>
                        <td>foundation_damage_characteristics</td>
                        <td>{string.Join(",", input.FoundationDamageCharacteristics)}</td>
                    </tr>
                    <tr>
                        <td>environment</td>
                        <td>{string.Join(",", input.EnvironmentDamageCharacteristics)}</td>
                    </tr>
                </tbody>
            </table>";

            await _emailService.SendAsync(mailTo: "info@kcaf.nl", mailCc: "info@laixer.com", null, "Incident reported", body, isHtml: true);
        }
    }
}
