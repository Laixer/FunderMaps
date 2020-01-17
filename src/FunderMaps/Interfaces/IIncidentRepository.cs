using FunderMaps.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Interfaces
{
    // TODO: Move to core
    public interface IIncidentRepository
    {
        Task SaveIncidentAsync(IncidentInputViewModel model);
    }
}
