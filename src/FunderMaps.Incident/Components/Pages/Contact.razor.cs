using Microsoft.AspNetCore.Components;
using FunderMaps.Core.IncidentReport;
using FunderMaps.Incident.Components.Layout;

namespace FunderMaps.Incident.Components.Pages;

public partial class Contact : ComponentBase, IDisposable
{
    [Inject]
    private IIncidentService IncidentService { get; set; } = default!;

    [CascadingParameter]
    State State { get; set; } = default!;

    [CascadingParameter]
    MainLayout Parent { get; set; } = default!;

    async Task ClickHandler()
    {
        if (!State.Feedback)
        {
            await IncidentService.AddAsync(State.Model);
        }
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
