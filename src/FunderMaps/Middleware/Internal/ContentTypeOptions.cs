﻿namespace FunderMaps.Middleware.Internal
{
    /// <summary>
    /// X-Content-Type-Options-related constants.
    /// </summary>
    public static class ContentTypeOptions
    {
        /// <summary>
        /// Header value for X-Content-Type-Options
        /// </summary>
        public static readonly string Header = "X-Content-Type-Options";

        /// <summary>
        /// Disables content sniffing
        /// </summary>
        public static readonly string NoSniff = "nosniff";
    }
}
