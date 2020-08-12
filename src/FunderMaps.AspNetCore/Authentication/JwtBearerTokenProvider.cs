using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Jwt bearer token provider.
    /// </summary>
    public class JwtBearerTokenProvider : ISecurityTokenProvider
    {
        /// <summary>
        ///     The <see cref="JwtBearerOptions"/> used.
        /// </summary>
        public JwtBearerOptions Options { get; private set; }

        /// <summary>
        ///     Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        ///     System clock.
        /// </summary>
        protected ISystemClock Clock { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public JwtBearerTokenProvider(IOptionsMonitor<JwtBearerOptions> options, ILogger<JwtBearerTokenProvider> logger, ISystemClock clock)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Get(JwtBearerDefaults.AuthenticationScheme);
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        protected virtual SecurityToken GenerateSecurityToken(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var properties = new AuthenticationProperties();

            var issuerSigningKey = Options.TokenValidationParameters.IssuerSigningKey;
            var SigningCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>(principal.Claims)
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // TODO: Otherwise email
            var nameClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.Ordinal));
            if (nameClaim != null)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameClaim.Value));
            }

            DateTimeOffset issuedUtc;
            if (properties.IssuedUtc.HasValue)
            {
                issuedUtc = properties.IssuedUtc.Value;
            }
            else
            {
                issuedUtc = Clock.UtcNow;
                properties.IssuedUtc = issuedUtc;
            }

            if (!properties.ExpiresUtc.HasValue)
            {
                properties.ExpiresUtc = issuedUtc.Add(TimeSpan.FromHours(2)); // issuedUtc.Add(Options.ExpireTimeSpan);
            }

            return new JwtSecurityToken(
                issuer: Options.TokenValidationParameters.ValidIssuer,
                audience: Options.TokenValidationParameters.ValidAudience,
                claims: claims,
                notBefore: properties.IssuedUtc?.LocalDateTime,
                expires: properties.ExpiresUtc?.LocalDateTime,
                signingCredentials: SigningCredentials);
        }

        /// <summary>
        ///     Generate token.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Returns token as <see cref="SecurityToken"/>.</returns>
        public virtual Task<SecurityToken> GetToken(ClaimsPrincipal principal)
            => Task.FromResult(GenerateSecurityToken(principal));

        /// <summary>
        ///     Generate token and return as string.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Returns token as string.</returns>
        public virtual async Task<string> GetTokenAsStringAsync(ClaimsPrincipal principal)
        {
            SecurityToken token = await GetToken(principal);

            // TODO: Move to method.
            SecurityTokenHandler handler = (SecurityTokenHandler)Options.SecurityTokenValidators.FirstOrDefault(s => (s as SecurityTokenHandler).CanWriteToken);
            if (handler == null)
            {
                throw new InvalidOperationException();
            }

            return handler.WriteToken(token);
        }
    }
}
