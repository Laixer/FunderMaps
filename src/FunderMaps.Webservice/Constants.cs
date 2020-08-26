using System;
using System.Reflection;

namespace FunderMaps.Webservice
{
    /// <summary>
    ///     Contains application constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        ///     Retrieve application name.
        /// </summary>
        internal static string ApplicationName => Assembly.GetEntryAssembly().GetName().Name;

        /// <summary>
        ///     Version.
        /// </summary>
        internal static Version ApplicationVersion => Assembly.GetEntryAssembly().GetName().Version;
    }
}
