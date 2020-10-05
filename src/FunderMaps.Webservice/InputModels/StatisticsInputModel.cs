using FunderMaps.AspNetCore.InputModels;
using FunderMaps.Core.Types.Products;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Webservice.InputModels
{
    /// <summary>
    ///     DTO for statistics request.
    /// </summary>
    public sealed class StatisticsInputModel : PaginationInputModel
    {
        /// <summary>
        ///     Product type.
        /// </summary>
        [Required]
        public StatisticsProductType? Product { get; set; }

        /// <summary>
        ///     Incidates the area code for the calculations.
        /// </summary>
        [Required]
        public string NeighborhoodCode { get; set; }
    }
}
