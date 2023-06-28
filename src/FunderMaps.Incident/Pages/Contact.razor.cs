using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Data;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Incident.Shared;

namespace FunderMaps.Incident.Pages;

public partial class Contact : ComponentBase, IDisposable
{
    [Inject]
    private ILogger<Finish> Logger { get; set; } = default!;

    [Inject]
    private IIncidentService incidentService { get; set; } = default!;

    [CascadingParameter]
    State State { get; set; } = default!;

    [CascadingParameter]
    MainLayout Parent { get; set; } = default!;

    async Task ClickHandler()
    {
        await incidentService.AddAsync(State.Model);
    }

    void ValidateModel()
    {
        if (!string.IsNullOrEmpty(State.Model.Email) && !string.IsNullOrEmpty(State.Model.Name))
        {
            State.DisableNavNext = false;
            Parent.Kaas();
        }
        else
        {
            State.DisableNavNext = true;
            Parent.Kaas();
        }
    }

    protected override void OnInitialized()
    {
        State.OnNextClick = EventCallback.Factory.Create(this, ClickHandler);
    }

    public void Dispose()
    {
        State.OnNextClick = EventCallback.Empty;
    }
}
