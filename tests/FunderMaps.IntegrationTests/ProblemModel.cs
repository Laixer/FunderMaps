namespace FunderMaps.IntegrationTests
{
    /// <summary>
    ///     Model returned when problem is encountered.
    /// </summary>
    public class ProblemModel
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public short Status { get; set; }

        public string TraceId { get; set; }
    }
}
