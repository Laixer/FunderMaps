using System.ComponentModel.DataAnnotations;
using FunderMaps.Core;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Pagination model.
    /// </summary>
    /// <remarks>
    ///     Used to paginate datasets.
    /// </remarks>
    public record PaginationDto
    {
        /// <summary>
        ///     Recordset offset.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     Recordset limit.
        /// </summary>
        [Range(1, 10000)]
        public int Limit { get; set; } = 25;

        /// <summary>
        ///     Sort onto field.
        /// </summary>
        public string SortOn { get; set; }

        /// <summary>
        ///     Sort ascending or descending.
        /// </summary>
        public bool SortAscending { get; set; } = true;

        /// <summary>
        ///     Get navigation from pagination.
        /// </summary>
        public Navigation Navigation => new()
        {
            Offset = Offset,
            Limit = Limit != 0 ? Limit : 100,
            SortColumn = SortOn,
            SortOrder = SortAscending ? SortOrder.Ascending : SortOrder.Descending,
        };
    }
}
