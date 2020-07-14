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

            try
            {
                return await GetAndBuildAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Retrieve all incidents.
        /// </summary>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<Incident> GetAllAsync(INavigation navigation)
        {
            // HACK: Cannot yield within a try/catch block as of C# 8.0 2020.
            ValueTask<Incident> func(Incident incident)
            {
                try
                {
                    return BuildAsync(incident);
                }
                catch (RepositoryException)
                {
                    // FUTURE: We *assume* repository exceptions are non existing entities.
                    throw new EntityNotFoundException();
                }
            }

            IAsyncEnumerable<Incident> asyncEnumerable = _incidentRepository.ListAllAsync(navigation);
            IAsyncEnumerator<Incident> e = asyncEnumerable.GetAsyncEnumerator();

        // TODO: Workaround for yield within a try/catch
        // HACK: This is a C# compiler/language bug, see https://github.com/dotnet/roslyn/issues/39583
        MoveToNext:
            try
            {
                bool hasResult = await e.MoveNextAsync().ConfigureAwait(false);
                if (!hasResult)
                {
                    yield break;
                }
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }

            yield return await func(e.Current).ConfigureAwait(false);
            goto MoveToNext;

            //await foreach (var incident in _incidentRepository.ListAllAsync(navigation))
            //{
            //    yield return await func(incident).ConfigureAwait(false);
            //}
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

            incident.Id = null;
            incident.AuditStatus = Types.AuditStatus.Todo;
            incident.CreateDate = DateTime.MinValue;
            incident.UpdateDate = null;
            incident.DeleteDate = null;
            incident.AddressNavigation = null;

            Validator.ValidateObject(incident, new ValidationContext(incident), true);

            try
            {
                // There does not have to be a contact, but if it exists we'll save it.
                if (incident.ContactNavigation != null)
                {
                    await _contactRepository.AddAsync(incident.ContactNavigation).ConfigureAwait(false);
                }

                var id = await _incidentRepository.AddAsync(incident).ConfigureAwait(false);
                return await GetAndBuildAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }

        /// <summary>
        ///     Update incident.
        /// </summary>
        /// <param name="incident">Entity object.</param>
        /// <exception cref="ValidationException">When <paramref name="incident" /> is found to be invalid.</exception>
        /// <exception cref="EntityNotFoundException">When <paramref name="incident" /> is not found.</exception>
        public virtual async ValueTask UpdateAsync(Incident incident)
        {
            Validator.ValidateObject(incident, new ValidationContext(incident), true);

            try
            {
                await _incidentRepository.UpdateAsync(incident).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
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

            try
            {
                await _incidentRepository.DeleteAsync(id).ConfigureAwait(false);
            }
            catch (RepositoryException)
            {
                // FUTURE: We *assume* repository exceptions are non existing entities.
                throw new EntityNotFoundException();
            }
        }
    }
}
