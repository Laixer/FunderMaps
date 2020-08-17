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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
namespace FunderMaps.IntegrationTests.Repositories
{
    /// <summary>
    ///     Mockup <see cref="AnalysisProduct"/> repository.
    /// </summary>
    public class TestAnalysisRepository : TestObjectRepositoryBase<AnalysisProduct>, IAnalysisRepository
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public TestAnalysisRepository(ObjectDataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<AnalysisProduct>> GetAllInFenceAsync(Guid userId, INavigation navigation, CancellationToken token) => throw new NotImplementedException();

        public async Task<AnalysisProduct> GetByExternalIdAsync(Guid userId, string externalId, ExternalDataSource externalSource, CancellationToken token)
        {
            var selected = DataStore.GetObjectsFromType<AnalysisProduct>().Where(x =>
            {
                bool kaas = x.ExternalId == externalId;
                bool worst = x.ExternalSource == externalSource;
                return kaas && worst;
            });

            var first = selected.FirstOrDefault();
            return first ?? throw new EntityNotFoundException();
        }

        public async Task<AnalysisProduct> GetByIdAsync(Guid userId, string id, CancellationToken token)
            => DataStore.GetObjectsFromType<AnalysisProduct>().Where(x => x.Id == id).First() ?? throw new EntityNotFoundException();

        public async Task<IEnumerable<AnalysisProduct>> GetByQueryAsync(Guid userId, string query, INavigation navigation, CancellationToken token)
        {
            if (DataStore.GetObjectsFromType<AnalysisProduct>().ToList().Count == 0)
            {
                throw new InvalidOperationException("Datastore contains no objects of type AnalysisProduct");
            }

            var result = new List<AnalysisProduct>();
            var random = new Random();
            for (int i = 0; i < navigation.Limit; i++)
            {
                result.Add(DataStore.GetObjectsFromType<AnalysisProduct>().ToArray()[random.Next(DataStore.GetObjectsFromType<AnalysisProduct>().ToList().Count)]);
            }

            await Task.CompletedTask;
            return result;
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
