namespace FunderMaps.Webservice.ResponseModels
{
    /// <summary>
    /// Base class for analysis endpoint responses.
    /// </summary>
    public class AnalysisResponseModelBase : ResponseModelBase
    {
        /// <summary>
        ///     Internal neighborhood id in which this building lies.
        /// </summary>
        public string NeighborhoodId { get; set; }
    }
}
