using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces
{
    public interface IAsyncRepository<TEntry>
    {
        Task<TEntry> GetByIdAsync(int id);
        Task<IReadOnlyList<TEntry>> ListAllAsync();
        Task<TEntry> AddAsync(TEntry entity);
        Task UpdateAsync(TEntry entity);
        Task DeleteAsync(TEntry entity);
        Task<int> CountAsync();
    }
}
