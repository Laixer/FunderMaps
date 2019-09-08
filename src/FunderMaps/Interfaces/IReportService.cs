using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    /// <summary>
    /// Report service.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Create new report.
        /// </summary>
        Task CreateAsync();

        /// <summary>
        /// Delete report.
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// Validate report.
        /// </summary>
        Task ValidateAsync();
    }
}
