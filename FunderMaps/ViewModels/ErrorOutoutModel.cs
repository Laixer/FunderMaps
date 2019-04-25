using System;

namespace FunderMaps.ViewModels
{
    public sealed class ErrorOutoutModel
    {
        /// <summary>
        /// Descriptive error message.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Request identifier,
        /// </summary>
        public string TraceId { get; set; }
    }
}
