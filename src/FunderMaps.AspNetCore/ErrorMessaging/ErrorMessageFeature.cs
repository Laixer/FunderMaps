using FunderMaps.Core.Interfaces;

namespace FunderMaps.AspNetCore.ErrorMessaging
{
    /// <summary>
    ///     Contains details about an error message.
    /// </summary>
    public class ErrorMessageFeature : IErrorMessage
    {
        /// <summary>
        ///     Message to display.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     HTTP Status code for this error message.
        /// </summary>
        public int StatusCode { get; set; }
    }
}
