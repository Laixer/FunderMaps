using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Types.Products;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Webservice.InputModels
{
    /// <summary>
    ///     DTO for an analysis product request.
    /// </summary>
    public sealed record AnalysisInputModel : PaginationDto
    {
        /// <summary>
        ///     Product type.
        /// </summary>
        [Required]
        public AnalysisProductType? Product { get; set; }

        /// <summary>
        ///     Input identifier.
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}
