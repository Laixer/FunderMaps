using System.Linq;

namespace FunderMaps.Webservice.Utility
{
    /// <summary>
    /// Contains utility functions regarding collections of arguments.
    /// </summary>
    internal static class ArgumentUtility
    {
        /// <summary>
        /// Returns the amount of items that are not null or emty in a list.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        internal static int NotNullCount(params string[] s) => s.Where(s => !string.IsNullOrEmpty(s)).Count();
    }
}
