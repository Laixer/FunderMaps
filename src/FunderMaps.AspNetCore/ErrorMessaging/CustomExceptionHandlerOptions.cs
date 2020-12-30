namespace FunderMaps.AspNetCore.ErrorMessaging
{
    /// <summary>
    ///     Options for <see cref="CustomExceptionHandlerMiddleware{TException}"/>.
    /// </summary>
    public record CustomExceptionHandlerOptions
    {
        /// <summary>
        ///     Path of the controller that handles our errors.
        /// </summary>
        public string ErrorControllerPath { get; init; }
    }
}
