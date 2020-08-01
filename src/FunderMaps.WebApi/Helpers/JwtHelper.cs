using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FunderMaps.WebApi.Helpers
{
    internal class JwtHelper
    {
        internal static SecurityKey CreateSecurityKey(string defaultKey = null)
        {
            if (!string.IsNullOrEmpty(defaultKey))
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(defaultKey));
            }

            // TODO: Write warning to log
            // TODO: Using IRandom
            using var rng = new Core.Services.RandomGenerator();
            return new SymmetricSecurityKey(rng.GetRandomByteArray(32));
        }
    }
}
