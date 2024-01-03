using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Components.Layout;
using FunderMaps.Core.Services;

namespace FunderMaps.Incident.Components.Pages;

public partial class Contact : ComponentBase, IDisposable
{
    [Inject]
    private IncidentService IncidentService { get; set; } = default!;

    [Inject]
    private FeedbackService FeedbackService { get; set; } = default!;

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
        else
        {
            await FeedbackService.AddAsync(State.Model);
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
