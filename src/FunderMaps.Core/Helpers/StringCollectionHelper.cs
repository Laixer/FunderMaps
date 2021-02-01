﻿using System.Linq;

namespace FunderMaps.Core.Helpers
{
    /// <summary>
    ///     Contains utility functions regarding collections of strings.
    /// </summary>
    public static class StringCollectionHelper
    {
        // FUTURE: Use any()
        /// <summary>
        ///     Returns the amount of items that are not null or emty in a list.
        /// </summary>
        /// <param name="s"></param>
        public static int NotNullCount(params string[] s) => s.Where(s => !string.IsNullOrEmpty(s)).Count();
    }
}