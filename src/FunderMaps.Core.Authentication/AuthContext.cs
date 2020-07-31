using System.Security.Claims;

namespace FunderMaps.Core.Authentication
{
    public class AuthContext
    {
        public static AuthContext Failed = new AuthContext(AuthResult.Failed);
        public static AuthContext Success = new AuthContext(AuthResult.Success);
        public static AuthContext LockedOut = new AuthContext(AuthResult.LockedOut);
        public static AuthContext NotAllowed = new AuthContext(AuthResult.NotAllowed);

        public AuthResult Result { get; }
        public ClaimsPrincipal Principal { get; }
        public string Token { get; }

        public AuthContext(AuthResult result)
        {
            Result = result;
        }

        public AuthContext(AuthResult result, ClaimsPrincipal principal)
        {
            Result = result;
            Principal = principal;
        }
    }
}
