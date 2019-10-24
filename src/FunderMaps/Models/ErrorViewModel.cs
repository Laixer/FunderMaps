namespace FunderMaps.Models
{
    /// <summary>
    /// Error view model.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Http request identifier.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Whether request identifier is available.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}