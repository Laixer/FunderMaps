using System.Reflection;

namespace FunderMaps.Webservice
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
        internal static string ApplicationVersion => "@@VERSION@@";

        /// <summary>
        ///     Application commit.
        /// </summary>
        internal static string ApplicationCommit => "@@COMMIT@@";
    }
}
