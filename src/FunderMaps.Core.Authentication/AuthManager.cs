using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
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
        public UserManager UserManager { get; set; }

        /// <summary>
        ///     The <see cref="AuthenticationOptions"/> used.
        /// </summary>
        public AuthenticationOptions Options { get; set; }

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public AuthManager(UserManager userManager, OrganizationManager organizationManager, IOptions<AuthenticationOptions> optionsAccessor) //, ILogger<AuthManager> logger)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            Options = optionsAccessor?.Value ?? new AuthenticationOptions();
        }

        // TODO: Add role

        /// <summary>
        ///     Create <see cref="ClaimsPrincipal"/> for specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to create the principal for.</param>
        /// <param name="authenticationType">Authentication type to use in authentication scheme.</param>
        /// <param name="additionalClaims">Additional claims that will be stored in the claim.</param>
        /// <returns><see cref="ClaimsPrincipal"/>.</returns>
        public ClaimsPrincipal CreateUserPrincipal(User user, string authenticationType, IEnumerable<Claim> additionalClaims = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var identity = new ClaimsIdentity(authenticationType, ClaimTypes.Name, ClaimTypes.Role);
            if (user.Id != Guid.Empty)
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            }

            //token.AddClaim(ClaimTypes.OrganizationUser, organizationUser.OrganizationId);
            //token.AddClaim(ClaimTypes.OrganizationUserRole, organizationUser.Role);

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
            var claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return await UserManager.GetAsync(Guid.Parse(claim.Value));
            }
            claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (claim != null)
            {
                return await UserManager.GetByEmailAsync(claim.Value);
            }
            claim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (claim != null)
            {
                return await UserManager.GetByEmailAsync(claim.Value);
            }

            throw new InvalidOperationException(); // TODO:
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
        public virtual async Task<bool> CanSignInAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            //if (Options.SignIn.RequireConfirmedEmail && !(await UserManager.IsEmailConfirmedAsync(user)))
            //{
            //    //Logger.LogWarning(0, "User {userId} cannot sign in without a confirmed email.", await UserManager.GetUserIdAsync(user));
            //    return false;
            //}
            //if (Options.SignIn.RequireConfirmedPhoneNumber && !(await UserManager.IsPhoneNumberConfirmedAsync(user)))
            //{
            //    //Logger.LogWarning(1, "User {userId} cannot sign in without a confirmed phone number.", await UserManager.GetUserIdAsync(user));
            //    return false;
            //}
            // TODO: Remove
            //if (Options.SignIn.RequireConfirmedAccount && !(await _confirmation.IsConfirmedAsync(UserManager, user)))
            //{
            //    //Logger.LogWarning(4, "User {userId} cannot sign in without a confirmed account.", await UserManager.GetUserIdAsync(user));
            //    return false;
            //}

            //if (await UserManager.IsLockedOutAsync(user))
            //{
            //Logger.LogWarning(3, "User {userId} is currently locked out.", await UserManager.GetUserIdAsync(user));
            //return AuthContext.LockedOut;
            //}

            return true;
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="principal"/>.
        /// </summary>
        /// <param name="principal">The principal to sign in.</param>
        /// <param name="checkIfAuthenticated">Check if the current principal is authenticated.</param>
        /// <returns>Instance of <see cref="AuthContext"/>.</returns>
        public virtual async Task<AuthContext> SignInAsync(ClaimsPrincipal principal, bool checkIfAuthenticated, string authenticationType)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            if (!IsSignedIn(principal) && checkIfAuthenticated)
            {
                return AuthContext.NotAllowed;
            }

            User user = await GetUserAsync(principal).ConfigureAwait(false);
            if (user == null)
            {
                return AuthContext.Failed;
            }

            return await SignInAsync(user, authenticationType);
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <returns>Instance of <see cref="AuthContext"/>.</returns>
        public virtual async Task<AuthContext> SignInAsync(User user, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await CanSignInAsync(user))
            {
                return AuthContext.NotAllowed;
            }

            return new AuthContext(AuthResult.Success, CreateUserPrincipal(user, authenticationType));
        }

        /// <summary>
        ///     Attempts to sign in the specified <paramref name="userName"/> and <paramref name="password"/> combination.
        /// </summary>
        /// <param name="email">The user email to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="AuthContext"/>.</returns>
        public virtual async Task<AuthContext> PasswordSignInAsync(string email, string password, string authenticationType)
        {
            User user = await UserManager.GetByEmailAsync(email);
            if (user == null)
            {
                return AuthContext.Failed;
            }

            return await PasswordSignInAsync(user, password, authenticationType);
        }

        /// <summary>
        /// Attempts a password sign in for a user.
        /// </summary>
        /// <param name="user">The user to sign in.</param>
        /// <param name="password">The password to attempt to sign in with.</param>
        /// <returns>Instance of <see cref="AuthContext"/>.</returns>
        public virtual async Task<AuthContext> PasswordSignInAsync(User user, string password, string authenticationType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!await CanSignInAsync(user))
            {
                return AuthContext.NotAllowed;
            }

            if (await UserManager.CheckPasswordAsync(user, password))
            {
                //var alwaysLockout = AppContext.TryGetSwitch("Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess", out var enabled) && enabled;
                //if (alwaysLockout)
                //{
                //return await UserManager.ResetAccessFailedCountAsync(user);
                //}

                return new AuthContext(AuthResult.Success, CreateUserPrincipal(user, authenticationType));
            }

            //Logger.LogWarning(2, "User {userId} failed to provide the correct password.", await UserManager.GetAsync(user.Id)); // TODO: Should be UserManager.GetAsync(user)'

            //await UserManager.IncreaseAccessFailedCountAsync(user);

            return AuthContext.Failed;
        }
    }
}
