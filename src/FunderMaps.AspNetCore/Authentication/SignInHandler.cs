using FunderMaps.Core.Authentication;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Helper for the signing process. Consolidates different authentication
    ///     services and provides a single facade.
    /// </summary>
    /// <remarks>
    ///     This is the only object which shpuld handle authentication and signing requests
    ///     for the entire web framework.
    /// </remarks>
    public class SignInHandler
    {
        private readonly ISecurityTokenProvider _tokenProvider;

        /// <summary>
        ///     The <see cref="SignInService"/> used.
        /// </summary>
        public SignInService SignInService { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SignInHandler(SignInService signInService, ISecurityTokenProvider tokenProvider)
        {
            SignInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        /// <summary>
        ///     Signin the user by email and password.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Security token if the authentication attempt was successful.</returns>
        public async Task<string> SignInAsync(string email, string password)
        {
            /// <summary>
            ///     Secure the backend signin call.
            /// </summary>
            /// <remarks>
            ///     Discrepanties in the input can lead to different behaviour further down
            ///     the call tree, possibly leaking information.
            ///     If any <see cref="FunderMapsCoreException"/> in thrown after this point
            ///     we must consider it an authentication failure.
            /// </remarks>
            async Task<SignInContext> PasswordSignInAsync()
            {
                try
                {
                    return await SignInService.PasswordSignInAsync(email, password, JwtBearerDefaults.AuthenticationScheme);
                }
                catch (FunderMapsCoreException)
                {
                    throw new AuthenticationException();
                }
            }

            var result = await PasswordSignInAsync();
            if (result.Result != AuthResult.Success)
            {
                throw new AuthenticationException();
            }

            if (result.Principal == null)
            {
                throw new AuthenticationException();
            }
            return await _tokenProvider.GetTokenAsStringAsync(result.Principal);
        }

        /// <summary>
        ///     Signin the user by active session.
        /// </summary>
        /// <param name="principal">Context principal.</param>
        /// <returns>Security token if the authentication attempt was successful.</returns>
        public async Task<string> RefreshSignInAsync(ClaimsPrincipal principal)
        {
            /// <summary>
            ///     Secure the backend signin call.
            /// </summary>
            /// <remarks>
            ///     Discrepanties in the input can lead to different behaviour further down
            ///     the call tree, possibly leaking information.
            ///     If any <see cref="FunderMapsCoreException"/> in thrown after this point
            ///     we must consider it an authentication failure.
            /// </remarks>
            async Task<SignInContext> SignInAsync()
            {
                try
                {
                    return await SignInService.SignInAsync(principal, JwtBearerDefaults.AuthenticationScheme);
                }
                catch (FunderMapsCoreException)
                {
                    throw new AuthenticationException();
                }
            }

            var result = await SignInAsync();
            if (result.Result != AuthResult.Success)
            {
                throw new AuthenticationException();
            }

            if (result.Principal == null)
            {
                throw new AuthenticationException();
            }
            return await _tokenProvider.GetTokenAsStringAsync(result.Principal);
        }
    }
}
