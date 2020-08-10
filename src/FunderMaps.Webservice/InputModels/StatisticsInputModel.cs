using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Webservice.InputModels
{
    /// <summary>
    ///     DTO for statistics request.
    ///     TODO This should check for invalid combinations.
    /// </summary>
    public sealed class StatisticsInputModel : PaginationInputModel
    {
        /// <summary>
        ///     Product type.
        /// </summary>
        [Required]
        public string Product { get; set; }

        /// <summary>
        ///     Incidates the area code for the calculations.
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        ///     Indicates whether or not we want the statistics for our entire fence.
        /// </summary>
        public bool FullFence { get; set; } = false;
    }
}
