namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Default implementation of an error message.
    /// </summary>
    public class ErrorMessage
    {
        /// <summary>
        ///     Error message text message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Error message status code.
        /// </summary>
        public int StatusCode { get; set; }
    }
}
