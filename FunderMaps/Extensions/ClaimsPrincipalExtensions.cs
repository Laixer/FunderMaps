using System.Linq;
using System.Security.Claims;

namespace FunderMaps.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Get the claim value by claim name.
        /// </summary>
        /// <param name="claimName">Claim key.</param>
        /// <returns>claim value.</returns>
        public static string GetClaim(this ClaimsPrincipal principal, string claimName)
        {
            var claim = principal.Claims.Where(s => s.Type == claimName).FirstOrDefault();
            if (claim == null) { return null; }

            return claim.Value;
        }
    }
}
