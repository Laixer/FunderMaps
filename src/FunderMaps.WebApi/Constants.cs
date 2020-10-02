namespace FunderMaps
{
    /// <summary>
    ///     Application constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        ///     Allowed file mime types in the application.
        /// </summary>
        /// <remarks>
        ///     These mime types are whitelisted. Every other
        ///     file type is not allowed in the application.
        ///     <para>
        ///         Controllers can take a subset of this list
        ///         for specific methods.
        ///     </para>
        /// </remarks>
        internal static readonly string[] AllowedFileMimes =
        {
            "application/pdf",

            "image/png",
            "image/jpeg",
            "image/gif",
            "image/bmp",
            "image/tiff",
            "image/webp",

            "text/plain",
        };

        /// <summary>
        ///     Incident gateway name.
        /// </summary>
        internal const string IncidentGateway = "FunderMaps.WebApi";
    }
}
