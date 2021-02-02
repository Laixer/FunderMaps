using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Notification;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.IncidentReport
{
    /// <summary>
    ///     Service to the incidents.
    /// </summary>
    internal class IncidentService : IIncidentService
    {
        private readonly IncidentOptions _options;
        private readonly Core.AppContext _appContext;
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;
        private readonly IGeocoderTranslation _geocoderTranslation;
        private readonly INotifyService _notifyService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentService(
            IOptions<IncidentOptions> options,
            Core.AppContext appContext,
            IContactRepository contactRepository,
            IIncidentRepository incidentRepository,
            IGeocoderTranslation geocoderTranslation,
            INotifyService notifyService)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _geocoderTranslation = geocoderTranslation ?? throw new ArgumentNullException(nameof(geocoderTranslation));
            _notifyService = notifyService ?? throw new ArgumentNullException(nameof(notifyService));
        }

        // FUTURE: split logic, hard to read.
        /// <summary>
        ///     Register a new incident.
        /// </summary>
        /// <param name="incident">Incident to process.</param>
        /// <param name="meta">Optional metadata.</param>
        public async Task<Incident> AddAsync(Incident incident, object meta = null)
        {
            Address address = await _geocoderTranslation.GetAddressIdAsync(incident.Address);

            incident.Address = address.Id;
            incident.ClientId = _options.ClientId;
            incident.Meta = meta;

            await _contactRepository.AddAsync(incident.ContactNavigation);
            var id = await _incidentRepository.AddAsync(incident);
            incident = await _incidentRepository.GetByIdAsync(id);
            incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);

            await _notifyService.DispatchNotifyAsync("incident_notify", new()
            {
                Recipients = _options.Recipients,
                Items = new Dictionary<string, string> { { "id", id } },
            });

            return incident;
        }
    }
}
