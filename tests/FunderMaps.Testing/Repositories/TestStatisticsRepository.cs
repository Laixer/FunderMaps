using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    /// <summary>
    ///     Mockup <see cref="StatisticsProduct"/> repository.
    /// </summary>
    public class TestStatisticsRepository : IStatisticsRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public DataStore<StatisticsProduct> DataStore { get; set; }

        public TestStatisticsRepository(DataStore<StatisticsProduct> dataStore)
        {
            DataStore = dataStore;
        }

        public async Task<StatisticsProduct> GetStatisticsByIdAsync(string id)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.First();
        }

        public async Task<StatisticsProduct> GetStatisticsByExternalIdAsync(string id)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.First();
        }
    }
}
