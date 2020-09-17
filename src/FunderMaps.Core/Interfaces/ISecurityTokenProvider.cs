using System.Security.Claims;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Security token provider.
    /// </summary>
    public interface ISecurityTokenProvider
    {
        /// <summary>
        ///     Generate token and return as string.
        /// </summary>
        /// <param name="principal">Claims principal.</param>
        /// <returns>Returns token as string.</returns>
        Task<string> GetTokenAsStringAsync(ClaimsPrincipal principal);
    }
}
