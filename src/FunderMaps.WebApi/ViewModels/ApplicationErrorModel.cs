namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// Model for error reporting.
    /// </summary>
    public class ApplicationErrorModel
    {
        /// <summary>
        /// Descriptive error message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Request identifier,
        /// </summary>
        public string TraceId { get; set; }
    }
}
