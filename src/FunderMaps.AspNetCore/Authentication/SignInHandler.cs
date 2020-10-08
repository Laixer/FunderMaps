﻿using FunderMaps.Core.Authentication;
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
            var result = await SignInService.PasswordSignInAsync(email, password, JwtBearerDefaults.AuthenticationScheme);
            if (result.Result != AuthResult.Success)
            {
                throw new AuthenticationException();
            }

            if (result.Principal == null)
            {
                throw new InvalidOperationException();
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
            var result = await SignInService.SignInAsync(principal, JwtBearerDefaults.AuthenticationScheme);
            if (result.Result != AuthResult.Success)
            {
                throw new AuthenticationException();
            }

            if (result.Principal == null)
            {
                throw new InvalidOperationException();
            }
            return await _tokenProvider.GetTokenAsStringAsync(result.Principal);
        }
    }
}