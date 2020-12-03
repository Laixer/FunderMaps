using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Testing.Repositories
{
    /// <summary>
    ///     Mockup <see cref="AnalysisProduct"/> repository.
    /// </summary>
    public class TestAnalysisRepository : IAnalysisRepository
    {
        /// <summary>
        ///     Datastore holding the entities.
        /// </summary>
        public DataStore<AnalysisProduct> DataStore { get; set; }

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestAnalysisRepository(DataStore<AnalysisProduct> dataStore)
        {
            DataStore = dataStore;
        }

        public async Task<AnalysisProduct> GetByExternalIdAsync(string externalId)
        {
            // NOTE: We ignore external data source here.
            await Task.CompletedTask;
            return DataStore.ItemList.Where(s => s.ExternalId == externalId).FirstOrDefault() ?? throw new EntityNotFoundException();
        }

        public Task<AnalysisProduct> GetByAddressExternalIdAsync(string externalId)
        {
            throw new NotImplementedException();
        }

        public async Task<AnalysisProduct> GetByIdAsync(string id)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.Id == id).First() ?? throw new EntityNotFoundException();
        }
    }
}
