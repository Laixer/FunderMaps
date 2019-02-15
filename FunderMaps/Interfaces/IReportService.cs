using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    public interface IReportService
    {
        Task CreateAsync();
        Task DeleteAsync();
        Task ValidateAsync();
    }
}
