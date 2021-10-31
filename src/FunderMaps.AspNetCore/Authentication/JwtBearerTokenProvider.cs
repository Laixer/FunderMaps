using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

#pragma warning disable CA1812 // Internal class is never instantiated
namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Jwt bearer token provider.
    /// </summary>
    internal class JwtBearerTokenProvider : ISecurityTokenProvider
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
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Get(JwtBearerDefaults.AuthenticationScheme);
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        /// <summary>
        ///     Find the first security token handler that can write a token.
        /// </summary>
        private SecurityTokenHandler Handler
            => (SecurityTokenHandler)Options.SecurityTokenValidators.FirstOrDefault(s => (s as SecurityTokenHandler).CanWriteToken);

        /// <summary>
        ///     Generate a <see cref="SecurityToken"/> from a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
        protected virtual SecurityToken GenerateSecurityToken(ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            AuthenticationProperties properties = new();

            var JwtTokenValidationParameters = Options.TokenValidationParameters as JwtTokenValidationParameters;
            var issuerSigningKey = JwtTokenValidationParameters.IssuerSigningKey;
            SigningCredentials SigningCredentials = new(issuerSigningKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new(principal.Claims)
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var nameClaim = claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name, StringComparison.Ordinal));
            if (nameClaim is not null)
            {
                claims.Add(new(JwtRegisteredClaimNames.Sub, nameClaim.Value));
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

            if (!properties.ExpiresUtc.HasValue && JwtTokenValidationParameters.Valid != TimeSpan.Zero)
            {
                properties.ExpiresUtc = issuedUtc.Add(JwtTokenValidationParameters.Valid);
            }

            return new JwtSecurityToken(
                issuer: JwtTokenValidationParameters.ValidIssuer,
                audience: JwtTokenValidationParameters.ValidAudience,
                claims: claims,
                notBefore: properties.IssuedUtc?.LocalDateTime,
                expires: properties.ExpiresUtc?.LocalDateTime,
                signingCredentials: SigningCredentials);
        }

        /// <summary>
        ///     Generate a <see cref="SecurityToken"/> from a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Instance of <see cref="SecurityToken"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual SecurityToken GetToken(ClaimsPrincipal principal)
            => GenerateSecurityToken(principal);

        /// <summary>
        ///     Generate a <see cref="TokenContext"/> from a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Instance of <see cref="TokenContext"/>.</returns>
        public virtual TokenContext GetTokenContext(ClaimsPrincipal principal)
        {
            if (Handler is null)
            {
                throw new InvalidOperationException();
            }

            SecurityToken token = GetToken(principal);
            return new()
            {
                TokenString = Handler.WriteToken(token),
                Token = token,
            };
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
