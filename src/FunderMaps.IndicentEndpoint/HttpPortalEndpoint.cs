using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.UseCases;
using FunderMaps.Data;
using FunderMaps.IndicentEndpoint.Extensions;
using FunderMaps.IndicentEndpoint.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.IndicentEndpoint
{
    /// <summary>
    /// Portal endpoint.
    /// </summary>
    public class HttpPortalEndpoint
    {
        private const string gatewayName = "FunderMaps.IndicentEndpoint";

        private readonly IncidentUseCase _incidentUseCase;
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public HttpPortalEndpoint(IncidentUseCase incidentUseCase, IAddressRepository addressRepository)
        {
            _incidentUseCase = incidentUseCase ?? throw new ArgumentNullException(nameof(incidentUseCase));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        /// <summary>
        /// Generate error result.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>See <see cref="ActionResult"/>.</returns>
        private static ActionResult ErrorResult(string message)
            => new JsonResult(new { Error = true, Message = message })
            {
                StatusCode = 400
            };

        /// <summary>
        /// Generate not found result.
        /// </summary>
        /// <param name="message">Not found message.</param>
        /// <returns>See <see cref="ActionResult"/>.</returns>
        private static ActionResult NotFoundResult(string message)
            => new JsonResult(new { Error = true, Message = message })
            {
                StatusCode = 404
            };

        /// <summary>
        /// Post an incident.
        /// </summary>
        /// <param name="input">See <see cref="IncidentInputViewModel"/>.</param>
        /// <returns>204 on success.</returns>
        [FunctionName("Incident")]
        public async Task<IActionResult> AddIncident([HttpTrigger(AuthorizationLevel.Function, "post")] IncidentInputViewModel input, HttpRequest request)
        {
            if (request == null)
            {
                return ErrorResult("Input is empty");
            }

            if (input == null)
            {
                return ErrorResult("Input is empty");
            }

            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(input, new ValidationContext(input, null, null), results, true))
            {
                return ErrorResult(string.Join(",", results));
            }

            // TODO: Refactor this to be more readable.
            var incident = new Incident().MapFrom(input);
            incident.Meta = new
            {
                UserAgent = request.UserAgent(),
                RemoteAddress = request.HttpContext.Connection.RemoteIpAddress.ToString(),
                Gateway = gatewayName,
            };

            await _incidentUseCase.CreateAsync(incident).ConfigureAwait(false);

            return new OkObjectResult(null);
        }

        /// <summary>
        /// Get address identifier based on input.
        /// </summary>
        /// <param name="request">See <see cref="AddressOutputViewModel"/>.</param>
        /// <returns>See <see cref="AddressOutputViewModel"/>.</returns>
        [FunctionName("Address")]
        public async Task<IActionResult> GetAddress([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest request)
        {
            if (request == null)
            {
                return ErrorResult("Input is empty");
            }

            string postcode = request.Query["postcode"];
            string buildingNumber = request.Query["BuildingNumber"];

            if (string.IsNullOrEmpty(postcode) || string.IsNullOrEmpty(buildingNumber))
            {
                return ErrorResult("Input is empty");
            }

            // TODO: Should yield single item
            await foreach (var address in _addressRepository.GetByPostalCodeAsync(postcode, buildingNumber, Navigation.SingleRow))
            {
                return new OkObjectResult(new AddressOutputViewModel
                {
                    Address = address.Id
                });
            }

            return NotFoundResult("Not found");
        }
    }
}
