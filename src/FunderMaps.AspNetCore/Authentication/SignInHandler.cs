using FunderMaps.Core.Authentication;
using FunderMaps.Core.Exceptions;
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
        /// <remarks>
        ///     Discrepanties in the input can lead to different behaviour further down
        ///     the call tree, possibly leaking information on exceptional cases.
        ///     If any <see cref="FunderMapsCoreException"/> is thrown we must consider
        ///     it an authentication failure.
        /// </remarks>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Security token if the authentication attempt was successful.</returns>
        public async Task<TokenContext> SignInAsync(string email, string password)
        {
            try
            {
                var result = await SignInService.PasswordSignInAsync(email, password, JwtBearerDefaults.AuthenticationScheme);
                if (result.Result != AuthResult.Success)
                {
                    throw new AuthenticationException();
                }

                if (result.Principal == null)
                {
                    throw new AuthenticationException();
                }
                return _tokenProvider.GetTokenContext(result.Principal);
            }
            catch (FunderMapsCoreException)
            {
                throw new AuthenticationException();
            }
        }

        /// <summary>
        ///     Signin the user by active session.
        /// </summary>
        /// <remarks>
        ///     Discrepanties in the input can lead to different behaviour further down
        ///     the call tree, possibly leaking information on exceptional cases.
        ///     If any <see cref="FunderMapsCoreException"/> is thrown we must consider
        ///     it an authentication failure.
        /// </remarks>
        /// <param name="principal">Context principal.</param>
        /// <returns>Security token if the authentication attempt was successful.</returns>
        public async Task<TokenContext> RefreshSignInAsync(ClaimsPrincipal principal)
        {
            try
            {
                var result = await SignInService.SignInAsync(principal, JwtBearerDefaults.AuthenticationScheme);
                if (result.Result != AuthResult.Success)
                {
                    throw new AuthenticationException();
                }

                if (result.Principal == null)
                {
                    throw new AuthenticationException();
                }
                return _tokenProvider.GetTokenContext(result.Principal);
            }
            catch (FunderMapsCoreException)
            {
                throw new AuthenticationException();
            }
        }
    }
}
