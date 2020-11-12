using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Address search DTO.
    /// </summary>
    public sealed class AddressSearchDto : PaginationDto // TODO: record
    {
        /// <summary>
        ///     Search query.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Query { get; init; }
    }
}
