namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Dataset statistics DTO.
    /// </summary>
    /// <remarks>
    ///     Statistics in this object can depend upon context,
    ///     scope and constraints and may not reflect the actual
    ///     items present within the dataset.
    /// </remarks>
    public class DatasetStatsDto
    {
        /// <summary>
        ///     Total number of ites in dataset.
        /// </summary>
        public long Count { get; set; }
    }
}
