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

            return await _organizationProposalRepository.GetByIdAsync(id);
        }

        public virtual async ValueTask<OrganizationProposal> GetProposalByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await _organizationProposalRepository.GetByNameAsync(name);
        }

        public virtual async ValueTask<OrganizationProposal> GetProposalByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _organizationProposalRepository.GetByEmailAsync(email);
        }

        public virtual async ValueTask<OrganizationProposal> CreateProposalAsync(OrganizationProposal organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults();
            organization.Validate();

            var id = await _organizationProposalRepository.AddAsync(organization);
            return await _organizationProposalRepository.GetByIdAsync(id);
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
            await _organizationProposalRepository.DeleteAsync(id);
        }

        #endregion Organization Proposal

        #region Organization

        public virtual async ValueTask<Organization> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _organizationRepository.GetByIdAsync(id);
        }

        public virtual async ValueTask<Organization> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await _organizationRepository.GetByNameAsync(name);
        }

        public virtual async ValueTask<Organization> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _organizationRepository.GetByEmailAsync(email);
        }

        public virtual async ValueTask<Organization> CreateAsync(Organization organization)
        {
            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            organization.InitializeDefaults();
            organization.Validate();

            var id = await _organizationRepository.AddAsync(organization);
            return await _organizationRepository.GetByIdAsync(id);
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
            await _organizationRepository.AddFromProposalAsync(id, user.Email, passwordHash);
            return await _organizationRepository.GetByIdAsync(id);
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

            organization.InitializeDefaults(await _organizationRepository.GetByIdAsync(organization.Id));
            organization.Validate();

            await _organizationRepository.UpdateAsync(organization);
        }

        public virtual async ValueTask DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _organizationRepository.DeleteAsync(id);
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
            await _organizationRepository.GetByIdAsync(id);

            // FUTURE: Do in 1 call.
            user = await _userManager.CreateAsync(user, plainPassword);
            await _organizationUserRepository.AddAsync(id, user.Id, role);
            return user;
        }

        /// <summary>
        ///     Return if the user is a member in the provided organization.
        /// </summary>
        /// <param name="id">Organization identifier.</param>
        /// <param name="user">User object to test</param>
        /// <returns>True if user exists in organization.</returns>
        public virtual async ValueTask<bool> IsUserInOrganizationAsync(Guid id, User user)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _organizationUserRepository.IsUserInOrganization(id, user.Id);
        }

        /// <summary>
        ///     Retrieve organization by user.
        /// </summary>
        /// <param name="user">User object to retrieve organization for.</param>
        public virtual async ValueTask<Organization> GetUserOrganizationAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var organizationId = await _organizationUserRepository.GetOrganizationByUserIdAsync(user.Id);
            return await GetAsync(organizationId);
        }

        /// <summary>
        ///     Retrieve organization user role.
        /// </summary>
        /// <param name="user">User object to retrieve organization for.</param>
        public virtual async ValueTask<OrganizationRole> GetUserOrganizationRoleAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _organizationUserRepository.GetOrganizationRoleByUserIdAsync(user.Id);
        }

        /// <summary>
        ///     Retrieve all users by organization.
        /// </summary>
        /// <param name="id">Organization id.</param>
        /// <param name="navigation">Recordset nagivation.</param>
        public virtual async IAsyncEnumerable<User> GetAllUserAsync(Guid id, INavigation navigation)
        {
            await foreach (var user in _organizationUserRepository.ListAllAsync(id))
            {
                yield return await _userManager.GetAsync(user);
            }
        }

        public virtual async ValueTask UpdateUserAsync(Guid id, User user)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Make sure organization and user exist.
            await IsUserInOrganizationAsync(id, user);

            user.InitializeDefaults(await _userManager.GetAsync(user.Id));
            user.Validate();

            await _userManager.UpdateAsync(user);
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
            await _organizationUserRepository.IsUserInOrganization(id, userId);
            await _userManager.DeleteAsync(userId);
        }

        #endregion Organization User
    }
}
