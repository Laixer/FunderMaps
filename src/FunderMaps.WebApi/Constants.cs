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
        ///     Static file cache retention.
        /// </summary>
        internal const int StaticFileCacheRetention = 60 * 60 * 24 * 30;

        /// <summary>
        ///     Application role for administrator
        /// </summary>
        internal const string AdministratorRole = "administrator";

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

        /// <summary>
        ///     Organization with member policy.
        /// </summary>
        internal const string OrganizationMemberPolicy = "OrganizationMember";

        /// <summary>
        ///     Organization member with write organization role.
        /// </summary>
        internal const string OrganizationMemberWritePolicy = "OrganizationMemberWrite";

        /// <summary>
        ///     Organization member with verify organization role.
        /// </summary>
        internal const string OrganizationMemberVerifyPolicy = "OrganizationMemberVerify";

        /// <summary>
        ///     Organization member with superuser organization role.
        /// </summary>
        internal const string OrganizationMemberSuperPolicy = "OrganizationMemberSuper";

        /// <summary>
        ///     Organization with member or administrator policy.
        /// </summary>
        internal const string OrganizationMemberOrAdministratorPolicy = "OrganizationMemberOrAdministrator";

        /// <summary>
        ///     Organization member with write organization role or administrator policy.
        /// </summary>
        internal const string OrganizationMemberWriteOrAdministratorPolicy = "OrganizationMemberWriteOrAdministrator";

        /// <summary>
        ///     Organization member with verify organization role or administrator policy.
        /// </summary>
        internal const string OrganizationMemberVerifyOrAdministratorPolicy = "OrganizationMemberVerifyOrAdministrator";

        /// <summary>
        ///     Organization member with superuser organization role or administrator policy.
        /// </summary>
        internal const string OrganizationMemberSuperOrAdministratorPolicy = "OrganizationMemberSuperOrAdministrator";
    }
}
