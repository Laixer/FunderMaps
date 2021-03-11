namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Model returned when problem is encountered.
    /// </summary>
    public class ProblemModel
    {
        /// <summary>
        ///     IETF error type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Error title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Error detail.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        ///     Error status code.
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        ///     Request trace identifier.
        /// </summary>
        public string TraceId { get; set; }
    }
}
