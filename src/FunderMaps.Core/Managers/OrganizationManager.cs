using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
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
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IOrganizationProposalRepository _organizationProposalRepository;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationManager(
            UserManager userManager,
            IOrganizationRepository organizationRepository,
            IOrganizationUserRepository organizationUserRepository,
            IOrganizationProposalRepository organizationProposalRepository,
            IPasswordHasher passwordHasher)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            _organizationProposalRepository = organizationProposalRepository ?? throw new ArgumentNullException(nameof(organizationProposalRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        #region Organization Proposal

        public virtual async ValueTask<OrganizationProposal> GetProposalAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _organizationProposalRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async ValueTask<OrganizationProposal> GetProposalByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await _organizationProposalRepository.GetByNameAsync(name).ConfigureAwait(false);
        }

        public virtual async ValueTask<OrganizationProposal> GetProposalByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _organizationProposalRepository.GetByEmailAsync(email).ConfigureAwait(false);
        }

        public virtual async ValueTask<OrganizationProposal> CreateProposalAsync(OrganizationProposal organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults();
            organization.Validate();

            var id = await _organizationProposalRepository.AddAsync(organization).ConfigureAwait(false);
            return await _organizationProposalRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual IAsyncEnumerable<OrganizationProposal> GetAllProposalAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            return _organizationProposalRepository.ListAllAsync(navigation);
        }

        public virtual async ValueTask DeleteProposalAsync(Guid id)
        {
            await _organizationProposalRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        #endregion Organization Proposal

        #region Organization

        public virtual async ValueTask<Organization> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await _organizationRepository.GetByNameAsync(name).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

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
            organization.Validate();

            var id = await _organizationRepository.AddAsync(organization).ConfigureAwait(false);
            return await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async ValueTask<Organization> CreateFromProposalAsync(Guid id, User user, string plainPassword)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(plainPassword))
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            user.InitializeDefaults();
            user.Validate();

            var passwordHash = _passwordHasher.HashPassword(plainPassword);
            await _organizationRepository.AddFromProposalAsync(id, user.Email, passwordHash).ConfigureAwait(false);
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
            organization.Validate();

            await _organizationRepository.UpdateAsync(organization).ConfigureAwait(false);
        }

        public virtual async ValueTask DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _organizationRepository.DeleteAsync(id).ConfigureAwait(false);
        }

        #endregion Organization

        #region Organization User

        public virtual async ValueTask<User> AddUserAsync(Guid id, User user, OrganizationRole role = OrganizationRole.Reader, string plainPassword = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.InitializeDefaults();
            user.Validate();

            // Make sure organization exists.
            await _organizationRepository.GetByIdAsync(id).ConfigureAwait(false);

            // TODO: Do in 1 call.
            user = await _userManager.CreateAsync(user, plainPassword).ConfigureAwait(false);
            await _organizationUserRepository.AddAsync(id, user.Id, role).ConfigureAwait(false);
            return user;
        }

        public virtual async ValueTask DeleteUserAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            // Make sure organization and user exist.
            await _organizationUserRepository.IsUserInOrganization(id, userId).ConfigureAwait(false);
            await _userManager.DeleteAsync(userId).ConfigureAwait(false);
        }

        #endregion Organization User
    }
}
