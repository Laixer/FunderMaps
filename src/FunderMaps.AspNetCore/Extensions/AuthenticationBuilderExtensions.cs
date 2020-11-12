using FunderMaps.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Extensions to the authentication builder.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        ///     Register <see cref="JwtBearerTokenProvider"/> as <see cref="ISecurityTokenProvider"/>.
        /// </summary>
        public static AuthenticationBuilder AddJwtBearerTokenProvider(this AuthenticationBuilder authenticationBuilder)
        {
            if (authenticationBuilder == null)
            {
                throw new ArgumentNullException(nameof(authenticationBuilder));
            }

            authenticationBuilder.Services.AddTransient<ISecurityTokenProvider, JwtBearerTokenProvider>();

            return authenticationBuilder;
        }
    }
}
