namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Represents a response model for the risk index endpoint.
    /// </summary>
    public record RiskIndexDto
    {
        /// <summary>
        ///     Whether or not the object has an increased risk.
        /// </summary>
        public bool IncreasedRisk { get; init; }
    }
}
