using FunderMaps.Core.Entities;
using FunderMaps.Core.Identity;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.Core.Authentication
{
    /// <summary>
    ///     Provides the APIs for authentication.
    /// </summary>
    public class SignInService
    {
        /// <summary>
        ///     The <see cref="IUserRepository"/> used.
        /// </summary>
        public IUserRepository UserRepository { get; }

        /// <summary>
        ///     The <see cref="IOrganizationUserRepository"/> used.
        /// </summary>
        public IOrganizationUserRepository OrganizationUserRepository { get; }

        /// <summary>
        ///     The <see cref="IOrganizationRepository"/> used.
        /// </summary>
        public IOrganizationRepository OrganizationRepository { get; }

        /// <summary>
        ///     The <see cref="IPasswordHasher"/> used.
        /// </summary>
        public IPasswordHasher PasswordHasher { get; }

        /// <summary>
        ///     Gets the <see cref="ILogger"/> used to log messages.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public SignInService(
            IUserRepository userRepository,
            IOrganizationUserRepository organizationUserRepository,
            IOrganizationRepository organizationRepository,
            IPasswordHasher passwordHasher,
            ILogger<SignInService> logger)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            OrganizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            OrganizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Returns a flag indicating whether the specified user can sign in.
        /// </summary>
        /// <param name="user">The user whose sign-in status should be returned.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation, containing a flag that is true
        ///     if the specified user can sign-in, otherwise false.
        /// </returns>
        public virtual async Task<SignInContext> CanSignInAsync(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (await UserRepository.IsLockedOutAsync(user.Id))
            {
                Logger.LogWarning(3, $"User {user} is currently locked out.");

                return SignInContext.LockedOut;
            }

            return SignInContext.Success;
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="principal"/>.
        /// </summary>
        /// <param name="principal">The principal to sign in.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> SignInAsync(ClaimsPrincipal principal, string authenticationType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var (user, organization) = PrincipalProvider.GetTenantUser<User, Organization>(principal);
            if (user == null || organization == null)
            {
                return SignInContext.Failed;
            }

            var claim = principal.FindFirst(FunderMapsAuthenticationClaimTypes.OrganizationRole);
            if (claim == null)
            {
                return SignInContext.Failed;
            }

            return await SignInAsync(user, organization, Enum.Parse<OrganizationRole>(claim.Value), authenticationType);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> SignInAsync(IUser user, ITenant organization, OrganizationRole organizationRole, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            var result = await CanSignInAsync(user);
            if (result != SignInContext.Success)
            {
                return result;
            }

            Logger.LogInformation(1, $"User {user} sign in was successful.");

            return new SignInContext(
                result: AuthResult.Success,
                principal: PrincipalProvider.CreateTenantUserPrincipal(user, organization, organizationRole, authenticationType));
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="email">The user email to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> PasswordSignInAsync(string email, string password, string authenticationType)
        {
            IUser user = await UserRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return SignInContext.Failed;
            }

            // TODO: Single call?
            var organizationId = await OrganizationUserRepository.GetOrganizationByUserIdAsync(user.Id);
            Organization organization = await OrganizationRepository.GetByIdAsync(organizationId);
            OrganizationRole organizationRole = await OrganizationUserRepository.GetOrganizationRoleByUserIdAsync(user.Id);
            return await PasswordSignInAsync(user, organization, organizationRole, password, authenticationType);
        }

        /// <summary>
        ///     Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> PasswordSignInAsync(
            IUser user,
            ITenant organization,
            OrganizationRole organizationRole,
            string password,
            string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            var result = await CanSignInAsync(user);
            if (result != SignInContext.Success)
            {
                return result;
            }

            var currentPasswordHash = await UserRepository.GetPasswordHashAsync(user.Id);
            if (PasswordHasher.IsPasswordValid(currentPasswordHash, password))
            {
                await UserRepository.ResetAccessFailed(user.Id);
                await UserRepository.RegisterAccess(user.Id);

                Logger.LogInformation(1, $"User {user} password sign in was successful.");

                return new SignInContext(
                    result: AuthResult.Success,
                    principal: PrincipalProvider.CreateTenantUserPrincipal(user, organization, organizationRole, authenticationType));
            }

            Logger.LogWarning(2, $"User {user} failed to provide the correct password.");

            await UserRepository.BumpAccessFailed(user.Id);

            return SignInContext.Failed;
        }
    }
}
