using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    /// <summary>
    ///     Use case to the incident entity.
    /// </summary>
    public class IncidentUseCase
    {
        private readonly IBlobStorageService _fileStorageService;
        private readonly IContactRepository _contactRepository;
        private readonly IIncidentRepository _incidentRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public IncidentUseCase(IBlobStorageService fileStorageService, IContactRepository contactRepository, IIncidentRepository incidentRepository)
        {
            _fileStorageService = fileStorageService;
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
            _incidentRepository = incidentRepository ?? throw new ArgumentNullException(nameof(incidentRepository));
        }

        private async ValueTask<Incident> GetAndBuildAsync(string id)
        {
            var incident = await _incidentRepository.GetByIdAsync(id);
            return await BuildAsync(incident);
        }

        // FUTURE: This is working, but not efficient
        /// <summary>
        ///     Build return object.
        /// </summary>
        /// <param name="incident">Single incident.</param>
        /// <returns>Incident with all navigation properties.</returns>
        protected async ValueTask<Incident> BuildAsync(Incident incident)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            if (!string.IsNullOrEmpty(incident.Email))
            {
                incident.ContactNavigation = await _contactRepository.GetByIdAsync(incident.Email);
            }

            // FUTURE:
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

            return await GetAndBuildAsync(id);
        }

        /// <summary>
        ///     Store document and return new name.
        /// </summary>
        /// <param name="stream">FIle stream.</param>
        /// <param name="fileName">Original file name.</param>
        /// <param name="contentType">Original file content type.</param>
        /// <returns></returns>
        public async ValueTask<string> StoreDocumentAsync(Stream stream, string fileName, string contentType)
        {
            string newFileName = IO.Path.GetUniqueName(fileName);
            await _fileStorageService.StoreFileAsync("incident-report", newFileName, contentType, stream); // TODO: store?
            return newFileName;
        }

        /// <summary>
        ///     Retrieve all incidents.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<Incident> GetAllAsync(INavigation navigation)
        {
            await foreach (var incident in _incidentRepository.ListAllAsync(navigation))
            {
                yield return await BuildAsync(incident);
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
                await _contactRepository.AddAsync(incident.ContactNavigation);
            }

            var id = await _incidentRepository.AddAsync(incident);
            return await GetAndBuildAsync(id);
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

            incident.InitializeDefaults(await _incidentRepository.GetByIdAsync(incident.Id));
            incident.Validate();

            await _incidentRepository.UpdateAsync(incident);
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

            await _incidentRepository.DeleteAsync(id);
        }
    }
}
