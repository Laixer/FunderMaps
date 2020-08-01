using FunderMaps.WebApi.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Address search DTO.
    /// </summary>
    public sealed class AddressSearchDto : PaginationModel
    {
        /// <summary>
        ///     Search query.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Query { get; set; }
    }
}
