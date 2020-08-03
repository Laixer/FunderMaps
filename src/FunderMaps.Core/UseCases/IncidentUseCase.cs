using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    // TODO: Memory caching.
    /// <summary>
    ///     Use case to the incident entity.
    /// </summary>
    public class IncidentUseCase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentUseCase(IContactRepository contactRepository, IIncidentRepository incidentRepository)
        {
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
        }

        private async ValueTask<Incident> GetAndBuildAsync(string id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id).ConfigureAwait(false);
            return await BuildAsync(incident).ConfigureAwait(false);
        }

        // TODO: This is working, but not efficient
        protected async ValueTask<Incident> BuildAsync(Incident incident)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            if (!string.IsNullOrEmpty(incident.Email))
            {
                incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email).ConfigureAwait(false);
            }

            // TODO:
            // incident.AddressNavigation = ...

            return incident;
        }

        /// <summary>
        ///     Get incident by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <exception cref="ValidationException">When <paramref name="id" /> is found to be invalid.</exception>
        /// <exception cref="EntityNotFoundException">When <paramref name="id" /> is not found.</exception>
        public virtual async ValueTask<Incident> GetAsync(string id)
        {
            Validator.ValidateValue(id, new ValidationContext(id), new List<IncidentAttribute> { new IncidentAttribute() });

            return await GetAndBuildAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        ///     Retrieve all incidents.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<Incident> GetAllAsync(INavigation navigation)
        {
            await foreach (var incident in _incidentRepository.ListAllAsync(navigation))
            {
                yield return await BuildAsync(incident).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Create new incident in the backstore.
        /// </summary>
        /// <remarks>
        ///     This method validates all incoming data.
        ///     This method sets defaults according to business logic rules.
        ///     All exception down the chain *must* be terminated here.
        /// </remarks>
        /// <param name="incident">The entity to create.</param>
        /// <exception cref="ValidationException">When <paramref name="incident" /> is found to be invalid.</exception>
        /// <exception cref="EntityNotFoundException">When <paramref name="incident" /> is not found.</exception>
        public virtual async ValueTask<Incident> CreateAsync(Incident incident)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            incident.InitializeDefaults();
            incident.Validate();

            // There does not have to be a contact, but if it exists we'll save it.
            if (incident.ContactNavigation != null)
            {
                await _contactRepository.AddAsync(incident.ContactNavigation).ConfigureAwait(false);
            }

            var id = await _incidentRepository.AddAsync(incident).ConfigureAwait(false);
            return await GetAndBuildAsync(id).ConfigureAwait(false);
        }

        /// <summary>
        ///     Update incident.
        /// </summary>
        /// <param name="incident">Entity object.</param>
        /// <exception cref="ValidationException">When <paramref name="incident" /> is found to be invalid.</exception>
        /// <exception cref="EntityNotFoundException">When <paramref name="incident" /> is not found.</exception>
        public virtual async ValueTask UpdateAsync(Incident incident)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            incident.InitializeDefaults(await _incidentRepository.GetByIdAsync(incident.Id).ConfigureAwait(false));
            incident.Validate();

            await _incidentRepository.UpdateAsync(incident).ConfigureAwait(false);
        }

        /// <summary>
        ///     Delete entity with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <exception cref="ValidationException">When <paramref name="id" /> is found to be invalid.</exception>
        /// <exception cref="EntityNotFoundException">When <paramref name="id" /> is not found.</exception>
        public virtual async ValueTask DeleteAsync(string id)
        {
            Validator.ValidateValue(id, new ValidationContext(id), new List<IncidentAttribute> { new IncidentAttribute() });

            await _incidentRepository.DeleteAsync(id).ConfigureAwait(false);
        }
    }
}
