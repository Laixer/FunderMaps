using Microsoft.AspNetCore.Components;

namespace FunderMaps.Incident.Components.Pages;

public partial class Finish : ComponentBase
{
    [Inject]
    private ILogger<Finish> Logger { get; set; } = default!;

    [CascadingParameter]
    State? State { get; set; }
}
