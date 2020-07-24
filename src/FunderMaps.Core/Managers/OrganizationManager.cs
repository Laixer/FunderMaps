using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Core.Managers
{
    /// <summary>
    ///     Organization manager.
    /// </summary>
    public class OrganizationManager
    {
        private readonly UserManager _userManager;
        private readonly IOrganizationRepository _organizationRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationManager(UserManager userManager, IOrganizationRepository organizationRepository)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        }

        #region Organization Proposal

        public virtual async ValueTask<Organization> CreateProposalAsync(Organization organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults();

            Validator.ValidateObject(organization, new ValidationContext(organization), true);

            var id = await _organizationRepository.AddAsync(organization).ConfigureAwait(false);
            return await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        #endregion // Organization Proposal

        #region Organization

        public virtual async ValueTask<Organization> GetAsync(Guid id)
        {
            return await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> GetByNameAsync(string name)
        {
            // TODO:
            //Validator.ValidateValue(name, new ValidationContext(name), new List<RequiredAttribute> { new RequiredAttribute() });

            return await _organizationRepository.GetByNameAsync(name).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> GetByEmailAsync(string email)
        {
            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _organizationRepository.GetByEmailAsync(email).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> CreateAsync(Organization organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults();

            Validator.ValidateObject(organization, new ValidationContext(organization), true);

            var id = await _organizationRepository.AddAsync(organization).ConfigureAwait(false);
            return await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual IAsyncEnumerable<Organization> GetAllAsync(INavigation navigation)
        {
            return _organizationRepository.ListAllAsync(navigation);
        }

        public virtual async ValueTask UpdateAsync(Organization organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults(await _organizationRepository.GetByIdAsync(organization.Id).ConfigureAwait(false));

            Validator.ValidateObject(organization, new ValidationContext(organization), true);

            await _organizationRepository.UpdateAsync(organization).ConfigureAwait(false);
        }

        public virtual async ValueTask DeleteAsync(Guid id)
        {
            await _organizationRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        #endregion // Organization

        #region Organization User

        #endregion // Organization User
    }
}
