using FunderMaps.Core.Interfaces;
using FunderMaps.Data;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    ///     Pagination model.
    /// </summary>
    /// <remarks>
    ///     Used to paginate datasets.
    /// </remarks>
    public class PaginationModel
    {
        /// <summary>
        ///     Recordset offset.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        ///     Recordset limit.
        /// </summary>
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
        public INavigation Navigation
        {
            get => new Navigation
            {
                Offset = Offset,
                Limit = Limit != 0 ? Limit : 100,
                SortColumn = SortOn,
                SortOrder = SortAscending ? SortOrder.Ascending : SortOrder.Descending,
            };
        }
    }
}
