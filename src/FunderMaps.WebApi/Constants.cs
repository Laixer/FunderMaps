using System;
using System.Reflection;

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
        ///     Retrieve application version.
        /// </summary>
        internal static Version ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version;

        /// <summary>
        ///     Retrieve application name.
        /// </summary>
        internal static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        ///     Report storage destination.
        /// </summary>
        internal const string ReportStorage = "report";

        /// <summary>
        ///     Evidence storage destination.
        /// </summary>
        internal const string EvidenceStorage = "evidence";
    }
}
