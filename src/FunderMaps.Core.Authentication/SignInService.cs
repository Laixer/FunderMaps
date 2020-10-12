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
        /// <param name="id">The user id whose sign-in status should be returned.</param>
        /// <returns><c>True</c> if the specified user can sign-in, otherwise false.</returns>
        public virtual async Task<SignInContext> CanSignInAsync(Guid id)
        {
            if (await UserRepository.IsLockedOutAsync(id))
            {
                Logger.LogWarning($"User {id} is currently locked out.");

                return SignInContext.LockedOut;
            }

            return SignInContext.Success;
        }

        /// <summary>
        ///     Test if the provided password is valid for the user.
        /// </summary>
        /// <param name="id">The user id whose password should be checked.</param>
        /// <param name="password">The password to be checked against the user.</param>
        /// <returns><c>True</c> if the specified password is valid for the user, otherwise false.</returns>
        public virtual async Task<bool> CheckPasswordAsync(Guid id, string password)
        {
            var passwordHash = await UserRepository.GetPasswordHashAsync(id);
            return PasswordHasher.IsPasswordValid(passwordHash, password);
        }

        /// <summary>
        ///     Set the password for the user.
        /// </summary>
        /// <param name="id">The user id whose password should be set.</param>
        /// <param name="password">The password to be set on the user.</param>
        public virtual async Task SetPasswordAsync(Guid id, string password)
        {
            var passwordHash = PasswordHasher.HashPassword(password);
            await UserRepository.SetPasswordHashAsync(id, passwordHash);
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

            var (user, tenant) = PrincipalProvider.GetUserAndTenant<User, Organization>(principal);
            if (user == null || tenant == null)
            {
                return SignInContext.Failed;
            }

            var claim = principal.FindFirst(FunderMapsAuthenticationClaimTypes.TenantRole);
            if (claim == null)
            {
                return SignInContext.Failed;
            }

            return await SignInAsync(user, tenant, Enum.Parse<OrganizationRole>(claim.Value), authenticationType);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="user"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> SignInAsync(IUser user, ITenant tenant, OrganizationRole organizationRole, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            var result = await CanSignInAsync(user.Id);
            if (result != SignInContext.Success)
            {
                return result;
            }

            Logger.LogInformation($"User {user} sign in was successful.");

            return new SignInContext(
                result: AuthResult.Success,
                principal: PrincipalProvider.CreateTenantUserPrincipal(user, tenant, organizationRole, authenticationType));
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
            ITenant tenant,
            OrganizationRole organizationRole,
            string password,
            string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            var result = await CanSignInAsync(user.Id);
            if (result != SignInContext.Success)
            {
                return result;
            }

            if (await CheckPasswordAsync(user.Id, password))
            {
                await UserRepository.ResetAccessFailed(user.Id);
                await UserRepository.RegisterAccess(user.Id);

                Logger.LogInformation($"User {user} password sign in was successful.");

                return new SignInContext(
                    result: AuthResult.Success,
                    principal: PrincipalProvider.CreateTenantUserPrincipal(user, tenant, organizationRole, authenticationType));
            }

            Logger.LogWarning($"User {user} failed to provide the correct password.");

            await UserRepository.BumpAccessFailed(user.Id);

            return SignInContext.Failed;
        }
    }
}
