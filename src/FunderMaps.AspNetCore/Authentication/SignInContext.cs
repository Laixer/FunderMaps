using System.Security.Claims;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Context returned from various authentication operations.
    /// </summary>
    public record SignInContext
    {
        /// <summary>
        ///     Return result as Failed.
        /// </summary>
        public static SignInContext Failed { get; } = new SignInContext(AuthResult.Failed);

        /// <summary>
        ///     Return result as Success.
        /// </summary>
        public static SignInContext Success { get; } = new SignInContext(AuthResult.Success);

        /// <summary>
        ///     Return result as LockedOut.
        /// </summary>        
        public static SignInContext LockedOut { get; } = new SignInContext(AuthResult.LockedOut);

        /// <summary>
        ///     Return result as NotAllowed.
        /// </summary>
        public static SignInContext NotAllowed { get; } = new SignInContext(AuthResult.NotAllowed);

        /// <summary>
        ///     Authentication result.
        /// </summary>
        public AuthResult Result { get; }

        /// <summary>
        ///     Claims principal.
        /// </summary>
        public ClaimsPrincipal Principal { get; }

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
