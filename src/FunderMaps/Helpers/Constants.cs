using Microsoft.AspNetCore.Identity;
using System;
using System.Reflection;

namespace FunderMaps.Helpers
{
    /// <summary>
    /// Application constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Default password policy.
        /// </summary>
        public static readonly PasswordOptions PasswordPolicy = new PasswordOptions
        {
            RequireDigit = false,
            RequireLowercase = false,
            RequireNonAlphanumeric = false,
            RequireUppercase = false,
            RequiredLength = 6,
            RequiredUniqueChars = 1,
        };

        /// <summary>
        /// Default lockout policy.
        /// </summary>
        public static readonly LockoutOptions LockoutOptions = new LockoutOptions
        {
            DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30),
            MaxFailedAccessAttempts = 10,
        };

        /// <summary>
        /// Static file cache retention.
        /// </summary>
        public const int StaticFileCacheRetention = 60 * 60 * 24 * 30;

        /// <summary>
        /// Application role for administrator
        /// </summary>
        public const string AdministratorRole = "administrator";

        /// <summary>
        /// Retrieve application version.
        /// </summary>
        public static Version ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version;

        /// <summary>
        /// Retrieve application name.
        /// </summary>
        public static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        /// Report storage destination.
        /// </summary>
        public const string ReportStorage = "report";

        /// <summary>
        /// Evidence storage destination.
        /// </summary>
        public const string EvidenceStorage = "evidence";

        /// <summary>
        /// Organization with member policy.
        /// </summary>
        public const string OrganizationMemberPolicy = "OrganizationMember";

        /// <summary>
        /// Organization member with write organization role.
        /// </summary>
        public const string OrganizationMemberWritePolicy = "OrganizationMemberWrite";

        /// <summary>
        /// Organization member with verify organization role.
        /// </summary>
        public const string OrganizationMemberVerifyPolicy = "OrganizationMemberVerify";

        /// <summary>
        /// Organization member with superuser organization role.
        /// </summary>
        public const string OrganizationMemberSuperPolicy = "OrganizationMemberSuper";

        /// <summary>
        /// Organization with member or administrator policy.
        /// </summary>
        public const string OrganizationMemberOrAdministratorPolicy = "OrganizationMemberOrAdministrator";

        /// <summary>
        /// Organization member with write organization role or administrator policy.
        /// </summary>
        public const string OrganizationMemberWriteOrAdministratorPolicy = "OrganizationMemberWriteOrAdministrator";

        /// <summary>
        /// Organization member with verify organization role or administrator policy.
        /// </summary>
        public const string OrganizationMemberVerifyOrAdministratorPolicy = "OrganizationMemberVerifyOrAdministrator";

        /// <summary>
        /// Organization member with superuser organization role or administrator policy.
        /// </summary>
        public const string OrganizationMemberSuperOrAdministratorPolicy = "OrganizationMemberSuperOrAdministrator";
    }
}
