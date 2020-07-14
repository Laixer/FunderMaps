namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// Authentication result model.
    /// </summary>
    public sealed class AuthenticationOutputModel
    {
        /// <summary>
        /// Authentication principal.
        /// </summary>
        public PrincipalOutputModel Principal { get; set; }

        /// <summary>
        /// Token as encoded string.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token validity in seconds.
        /// </summary>
        public int TokenValid { get; set; }
    }
}
