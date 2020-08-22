namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     Contract for error message display.
    /// </summary>
    public interface IErrorMessage
    {
        /// <summary>
        ///     Message to display.
        /// </summary>
        string Message { get; }

        /// <summary>
        ///     HTTP Status code for this error message.
        /// </summary>
        int StatusCode { get; }
    }
}
