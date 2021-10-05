using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Identity;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.Services
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
        ///     The <see cref="ISecurityTokenProvider"/> used.
        /// </summary>
        public ISecurityTokenProvider TokenProvider { get; }

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
            ISecurityTokenProvider tokenProvider,
            ILogger<SignInService> logger)
        {
            UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            OrganizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            OrganizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
            PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            TokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Test if the provided password is valid for the user.
        /// </summary>
        /// <param name="id">The user id whose password should be checked.</param>
        /// <param name="password">The password to be checked against the user.</param>
        /// <returns><c>True</c> if the specified password is valid for the user, otherwise false.</returns>
        public virtual async Task<bool> CheckPasswordAsync(Guid id, string password)
        {
            string passwordHash = await UserRepository.GetPasswordHashAsync(id);
            return PasswordHasher.IsPasswordValid(passwordHash, password);
        }

        /// <summary>
        ///     Set the password for the user.
        /// </summary>
        /// <param name="id">The user id whose password should be set.</param>
        /// <param name="password">The plaintext password to be set on the user.</param>
        public virtual async Task SetPasswordAsync(Guid id, string password)
        {
            string passwordHash = PasswordHasher.HashPassword(password);
            await UserRepository.SetPasswordHashAsync(id, passwordHash);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="principal"/>.
        /// </summary>
        /// <param name="principal">The principal to sign in.</param>
        /// <returns>Instance of <see cref="TokenContext"/>.</returns>
        public virtual ValueTask<TokenContext> SignInAsync(ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var (user, tenant) = PrincipalProvider.GetUserAndTenant<User, Organization>(principal);
            if (user is null || tenant is null)
            {
                throw new AuthenticationException();
            }

            Claim claim = principal.FindFirst(FunderMapsAuthenticationClaimTypes.TenantRole);
            if (claim is null)
            {
                throw new AuthenticationException();
            }

            Logger.LogTrace($"User '{user}' sign in was successful.");

            principal = PrincipalProvider.CreateTenantUserPrincipal(user, tenant,
                Enum.Parse<OrganizationRole>(claim.Value),
                JwtBearerDefaults.AuthenticationScheme);
            return new(TokenProvider.GetTokenContext(principal));
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="email"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="email">The user email to sign in.</param>
        /// <param name="password">The password to attempt to authenticate.</param>
        /// <returns>Instance of <see cref="TokenContext"/>.</returns>
        public virtual async Task<TokenContext> PasswordSignInAsync(string email, string password)
        {
            if (await UserRepository.GetByEmailAsync(email) is not IUser user)
            {
                throw new AuthenticationException();
            }

            // FUTURE: Single call?
            var organizationId = await OrganizationUserRepository.GetOrganizationByUserIdAsync(user.Id);

            if (await CheckPasswordAsync(user.Id, password))
            {
                if (await UserRepository.GetAccessFailedCount(user.Id) > 10)
                {
                    Logger.LogWarning($"User '{user}' locked out.");

                    throw new AuthenticationException();
                }

                await UserRepository.ResetAccessFailed(user.Id);
                await UserRepository.RegisterAccess(user.Id);

                Logger.LogInformation($"User '{user}' password sign in was successful.");

                Organization organization = await OrganizationRepository.GetByIdAsync(organizationId);
                OrganizationRole organizationRole = await OrganizationUserRepository.GetOrganizationRoleByUserIdAsync(user.Id);

                ClaimsPrincipal principal = PrincipalProvider.CreateTenantUserPrincipal(user, organization,
                    organizationRole,
                    JwtBearerDefaults.AuthenticationScheme);
                return TokenProvider.GetTokenContext(principal);
            }

            Logger.LogWarning($"User '{user}' failed to provide the correct password.");

            await UserRepository.BumpAccessFailed(user.Id);

            throw new AuthenticationException();
        }
    }
}
