namespace FunderMaps.Core.IO
{
    /// <summary>
    ///     <see cref="System.IO.File"/> extensions.
    /// </summary>
    public static class File
    {
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
