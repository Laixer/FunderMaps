namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Error response model.
    /// </summary>
    public sealed class ErrorResponseModel
    {
        /// <summary>
        /// Explaination of the error.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Internal error code.
        /// </summary>
        public int InternalErrorCode { get; set; }
    }
}
