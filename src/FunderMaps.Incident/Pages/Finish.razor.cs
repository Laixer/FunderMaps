using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Data;

namespace FunderMaps.Incident.Pages;

public partial class Finish : ComponentBase
{
    [Inject]
    private ILogger<Finish> Logger { get; set; } = default!;

    [CascadingParameter]
    State? State { get; set; }
}
