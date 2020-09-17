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
        ///     Application name.
        /// </summary>
        internal static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        ///     Application revision.
        /// </summary>
        internal static string ApplicationVersion => "@@VERSION@@";

        /// <summary>
        ///     Application commit.
        /// </summary>
        internal static string ApplicationCommit => "@@COMMIT@@";

        /// <summary>
        ///     Incident gateway name.
        /// </summary>
        internal const string IncidentGateway = "FunderMaps.WebApi";

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
