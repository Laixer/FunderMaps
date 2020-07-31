using System.Security.Claims;

namespace FunderMaps.Core.Authentication
{
    public class SignInContext
    {
        public static SignInContext Failed = new SignInContext(AuthResult.Failed);
        public static SignInContext Success = new SignInContext(AuthResult.Success);
        public static SignInContext LockedOut = new SignInContext(AuthResult.LockedOut);
        public static SignInContext NotAllowed = new SignInContext(AuthResult.NotAllowed);

        public AuthResult Result { get; }
        public ClaimsPrincipal Principal { get; }
        public string Token { get; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SignInContext(AuthResult result) => Result = result;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public SignInContext(AuthResult result, ClaimsPrincipal principal)
        {
            Result = result;
            Principal = principal;
        }
    }
}
