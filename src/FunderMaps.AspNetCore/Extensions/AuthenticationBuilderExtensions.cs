using FunderMaps.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationBuilderExtensions
    {
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
