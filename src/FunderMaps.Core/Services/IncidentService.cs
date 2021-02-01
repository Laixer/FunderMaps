using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Services
{
    /// <summary>
    ///     Service to the incidents.
    /// </summary>
    public class IncidentService : IIncidentService
    {
        private readonly Core.AppContext _appContext;
        private readonly IConfiguration _configuration;
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IGeocoderTranslation _geocoderTranslation;
        private readonly INotifyService _notifyService;

        // FUTURE: IOptions
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentService(
            Core.AppContext appContext,
            IConfiguration configuration,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IGeocoderTranslation geocoderTranslation,
            INotifyService notifyService)
        {
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _geocoderTranslation = geocoderTranslation ?? throw new ArgumentNullException(nameof(geocoderTranslation));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
        }

        /// <summary>
        ///     Register a new incident.
        /// </summary>
        /// <param name="incident">Incident to process.</param>
        /// <param name="meta">Optional metadata.</param>
        public async Task<Incident> AddAsync(Incident incident, object meta = null)
        {
            Address address = await _geocoderTranslation.GetAddressIdAsync(incident.Address);

            incident.Address = address.Id;
            incident.ClientId = int.Parse(_configuration["Incident:ClientId"]);
            incident.Meta = meta;

            // FUTURE: Contact is required, remove the checks.
            // Act.
            // There does not have to be a contact, but if it exists we'll save it.
            if (incident.ContactNavigation is not null && !string.IsNullOrEmpty(incident.ContactNavigation.Email))
            {
                await _contactRepository.AddAsync(incident.ContactNavigation);
            }

            // Act.
            var id = await _incidentRepository.AddAsync(incident);
            incident = await _incidentRepository.GetByIdAsync(id);
            incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);

            // Act.
            await _notifyService.DispatchNotifyAsync("incident_notify", new()
            {
                Recipients = new List<string> { "info@fundermaps.com", "info@kcaf.nl" }, // TODO: Retrieve from config
                Items = new Dictionary<string, string> { { "id", id } },
            });

            return incident;
        }
    }
}
