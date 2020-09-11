using System;
using System.Reflection;

namespace FunderMaps.AspNetCore
{
    /// <summary>
    ///     Contains application constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Application revision.
        /// </summary>
        public static string ApplicationRevision => "@@VERSION@@";

        /// <summary>
        ///     Application commit.
        /// </summary>
        public static string ApplicationCommit => "@@COMMIT@@";
    }
}
