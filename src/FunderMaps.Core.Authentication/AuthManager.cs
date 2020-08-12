using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.Core.Authentication
{
    /// <summary>
    ///     Provides the APIs for authentication.
    /// </summary>
    public class AuthManager
    {
        /// <summary>
        ///     The <see cref="UserManager"/> used.
        /// </summary>
        public UserManager UserManager { get; }

        /// <summary>
        ///     The <see cref="OrganizationManager"/> used.
        /// </summary>
        public OrganizationManager OrganizationManager { get; }

        /// <summary>
        ///     The <see cref="AuthenticationOptions"/> used.
        /// </summary>
        public AuthenticationOptions Options { get; set; }

        /// <summary>
        ///     Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public AuthManager(
            UserManager userManager,
            OrganizationManager organizationManager,
            IOptions<AuthenticationOptions> optionsAccessor,
            ILogger<AuthManager> logger)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            OrganizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
            Options = optionsAccessor?.Value ?? new AuthenticationOptions();
            Logger = logger ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        /// <summary>
        ///     Create <see cref="ClaimsPrincipal"/> for specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to create the principal for.</param>
        /// <param name="organization">The organization user is a member of.</param>
        /// <param name="organizationRole">The user role in the organization.</param>
        /// <param name="authenticationType">Authentication type to use in authentication scheme.</param>
        /// <param name="additionalClaims">Additional claims that will be stored in the claim.</param>
        /// <returns><see cref="ClaimsPrincipal"/>.</returns>
        public static ClaimsPrincipal CreateUserPrincipal(
            User user,
            Organization organization,
            OrganizationRole organizationRole,
            string authenticationType,
            IEnumerable<Claim> additionalClaims = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            user.Validate();
            organization.Validate();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.Organization, organization.Id.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.OrganizationRole, organizationRole.ToString()),
            };

            var identity = new ClaimsIdentity(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);

            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    identity.AddClaim(claim);
                }
            }

            return new ClaimsPrincipal(identity);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new InvalidOperationException(); // TODO:
            }

            return await UserManager.GetAsync(Guid.Parse(claim.Value));
        }

        public async Task<Organization> GetOrganizationAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var claim = principal.Claims.FirstOrDefault(c => c.Type == FunderMapsAuthenticationClaimTypes.Organization);
            if (claim == null)
            {
                throw new InvalidOperationException(); // TODO:
            }

            User user = await GetUserAsync(principal);
            if (!await OrganizationManager.IsUserInOrganizationAsync(Guid.Parse(claim.Value), user))
            {
                throw new InvalidOperationException(); // TODO:
            }
            return await OrganizationManager.GetAsync(Guid.Parse(claim.Value));
        }

        /// <summary>
        ///     Returns true if the principal has an identity with the application identity.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns>True if the user is logged in with identity.</returns>
        public virtual bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal?.Identities != null && principal.Identities.Any(i => i.IsAuthenticated);
        }

        /// <summary>
        ///     Returns a flag indicating whether the specified user can sign in.
        /// </summary>
        /// <param name="user">The user whose sign-in status should be returned.</param>
        /// <returns>
        ///     The task object representing the asynchronous operation, containing a flag that is true
        ///     if the specified user can sign-in, otherwise false.
        /// </returns>
        public virtual async Task<SignInContext> CanSignInAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (await UserManager.IsLockedOutAsync(user))
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
        /// <param name="checkIfAuthenticated">Check if the current principal is authenticated.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> SignInAsync(ClaimsPrincipal principal, bool checkIfAuthenticated, string authenticationType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (!IsSignedIn(principal) && checkIfAuthenticated)
            {
                return SignInContext.NotAllowed;
            }

            User user = await GetUserAsync(principal);
            Organization organization = await GetOrganizationAsync(principal);
            if (user == null || organization == null)
            {
                return SignInContext.Failed;
            }

            // TODO:
            var claim = principal.Claims.FirstOrDefault(c => c.Type == FunderMapsAuthenticationClaimTypes.OrganizationRole);
            if (claim == null)
            {
                return SignInContext.Failed;
            }

            return await SignInAsync(user, organization, Enum.Parse<OrganizationRole>(claim.Value), authenticationType);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> SignInAsync(User user, Organization organization, OrganizationRole organizationRole, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            user.Validate();
            organization.Validate();

            var result = await CanSignInAsync(user);
            if (result != SignInContext.Success)
            {
                return result;
            }

            Logger.LogInformation(1, $"User {user} sign in was successful.");

            return new SignInContext(AuthResult.Success, CreateUserPrincipal(user, organization, organizationRole, authenticationType));
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="email">The user email to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> PasswordSignInAsync(string email, string password, string authenticationType)
        {
            User user = await UserManager.GetByEmailAsync(email);
            if (user == null)
            {
                return SignInContext.Failed;
            }

            Organization organization = await OrganizationManager.GetUserOrganizationAsync(user);
            OrganizationRole organizationRole = await OrganizationManager.GetUserOrganizationRoleAsync(user);
            return await PasswordSignInAsync(user, organization, organizationRole, password, authenticationType);
        }

        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="SignInContext"/>.</returns>
        public virtual async Task<SignInContext> PasswordSignInAsync(User user, Organization organization, OrganizationRole organizationRole, string password, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (organization == null)
            {
                throw new ArgumentNullException(nameof(organization));
            }

            user.Validate();
            organization.Validate();

            var result = await CanSignInAsync(user);
            if (result != SignInContext.Success)
            {
                return result;
            }

            if (await UserManager.CheckPasswordAsync(user, password))
            {
                await UserManager.ResetAccessFailedCountAsync(user);
                await UserManager.IncreaseAccessCountAsync(user);

                Logger.LogInformation(1, $"User {user} password sign in was successful.");

                return new SignInContext(AuthResult.Success, CreateUserPrincipal(user, organization, organizationRole, authenticationType));
            }

            Logger.LogWarning(2, $"User {user} failed to provide the correct password.");

            await UserManager.IncreaseAccessFailedCountAsync(user);

            return SignInContext.Failed;
        }
    }
}
