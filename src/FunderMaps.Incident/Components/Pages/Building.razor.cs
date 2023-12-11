using FunderMaps.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FunderMaps.Incident.Components.Pages;

public partial class Building : ComponentBase
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ILogger<Building> Logger { get; set; } = default!;

    [Inject]
    private IGeocoderTranslation GeocoderTranslation { get; set; } = default!;

    [CascadingParameter]
    State State { get; set; } = default!;

    [Parameter]
    public string? Id { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (string.IsNullOrEmpty(Id))
        {
            NavigationManager.NavigateTo("/survey/address", true);
        }
        else
        {
            try
            {
                var address = await GeocoderTranslation.GetAddressIdAsync(Id);

                State.Model.Address = address.Id;
                State.Model.Building = address.BuildingId ?? throw new ArgumentNullException(nameof(address.BuildingId));

                if (State.Feedback)
                {
                    NavigationManager.NavigateTo("/survey/feedback-characteristics", false);
                }
                else
                {
                    NavigationManager.NavigateTo("/survey/foundation-damage-cause", false);
                }
            }
            catch
            {
                NavigationManager.NavigateTo("/survey/address", true);
            }
        }
    }
}
