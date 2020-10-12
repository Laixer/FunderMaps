using System.Reflection;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Application constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        ///     Application name.
        /// </summary>
        internal static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        ///     Application revision.
        /// </summary>
        internal const string ApplicationVersion = "@@VERSION@@";

        /// <summary>
        ///     Application commit.
        /// </summary>
        internal const string ApplicationCommit = "@@COMMIT@@";
    }
}
