using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Address search DTO.
    /// </summary>
    public sealed record AddressSearchDto : PaginationDto
    {
        /// <summary>
        ///     Search query.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Query { get; init; }
    }
}
