using Microsoft.AspNetCore.Identity;
using System;
using System.Reflection;

namespace FunderMaps.Helpers
{
    /// <summary>
    /// Application constants.
    /// </summary>
    public static class Constants
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
        public const int StaticFileCacheRetention = 60;

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
    }
}
