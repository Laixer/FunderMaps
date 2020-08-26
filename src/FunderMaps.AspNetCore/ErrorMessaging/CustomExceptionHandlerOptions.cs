namespace FunderMaps.AspNetCore.ErrorMessaging
{
    /// <summary>
    ///     Options for <see cref="CustomExceptionHandlerMiddleware{TException}"/>.
    /// </summary>
    public class CustomExceptionHandlerOptions
    {
        /// <summary>
        ///     Path of the controller that handles our errors.
        /// </summary>
        public string ErrorControllerPath { get; set; }
    }
}
