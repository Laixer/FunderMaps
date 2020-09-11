using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation, CancellationToken token)
            => throw new NotImplementedException();

        public async Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalSource, CancellationToken token)
        {
            // NOTE: We ignore external data source here.
            await Task.CompletedTask;
            return DataStore.ItemList.Where(s => s.ExternalId == externalId).FirstOrDefault() ?? throw new EntityNotFoundException();
        }

        public async Task<AnalysisProduct> GetByIdAsync(Guid userId, string id, CancellationToken token)
        {
            await Task.CompletedTask;
            return DataStore.ItemList.Where(x => x.Id == id).First() ?? throw new EntityNotFoundException();
        }

        public async Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation, CancellationToken token)
        {
            if (DataStore.ItemList.ToList().Count == 0)
            {
                throw new InvalidOperationException("Datastore contains no objects of type AnalysisProduct");
            }

            var result = new List<AnalysisProduct>();
            var random = new Random();
            for (int i = 0; i < navigation.Limit; i++)
            {
                result.Add(DataStore.ItemList.ToArray()[random.Next(DataStore.ItemList.ToList().Count)]);
            }

            await Task.CompletedTask;
            return result;
        }
    }
}
