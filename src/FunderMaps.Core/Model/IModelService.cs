using System.Threading.Tasks;

namespace FunderMaps.Core.Model
{
    /// <summary>
    ///     Model service.
    /// </summary>
    public interface IModelService
    {
        /// <summary>
        ///     Update models.
        /// </summary>
        Task UpdateAllModelsAsync();
    }
}
