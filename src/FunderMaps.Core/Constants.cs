namespace FunderMaps.Core
{
    /// <summary>
    ///     Application Core constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Incident storage destination folder name.
        /// </summary>
        public const string IncidentStorageFolderName = "incident-report";

        /// <summary>
        ///     Inquiry storage destination folder name.
        /// </summary>
        public const string InquiryStorageFolderName = "inquiry-report";

        /// <summary>
        ///     Recovery storage destination folder name.
        /// </summary>
        public const string RecoveryStorageFolderName = "recovery-report";

        /// <summary>
        ///     Allowed file mime types in the application.
        /// </summary>
        /// <remarks>
        ///     These mime types are whitelisted. Every other
        ///     file type is not allowed in the application.
        /// </remarks>
        public const string AllowedFileMimes = @"
            application/pdf,
            image/png,
            image/jpeg,
            image/gif,
            image/bmp,
            image/tiff,
            image/webp,
            text/plain";
    }
}
