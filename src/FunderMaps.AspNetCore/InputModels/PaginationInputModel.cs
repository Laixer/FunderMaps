using FunderMaps.Core.Interfaces;
using FunderMaps.Data;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.InputModels
{
    /// <summary>
    ///     DTO for pagination.
    /// </summary>
    public class PaginationInputModel
    {
        /// <summary>
        ///     Recordset offset.
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        ///     Recordset limit.
        /// </summary>
        [Range(1, uint.MaxValue)]
        public int Limit { get; set; } = 25;

        /// <summary>
        ///     Sort onto field.
        /// </summary>
        public string SortOn { get; set; }

        /// <summary>
        ///     Sort ascending or descending.
        /// </summary>
        public bool SortAscending { get; set; } = true;

        // TODO This links to FunderMaps.Data which seems incorrect?
        /// <summary>
        ///     Get navigation from pagination.
        /// </summary>
        public INavigation Navigation => new Navigation
        {
            Offset = Offset,
            Limit = Limit != 0 ? Limit : 100,
            SortColumn = SortOn,
            SortOrder = SortAscending ? SortOrder.Ascending : SortOrder.Descending,
        };
    }
}
