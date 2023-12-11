using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Components.Layout;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Services;

namespace FunderMaps.Incident.Components.Pages;

public partial class Address : ComponentBase, IAsyncDisposable
{
    private ElementReference mapElement;
    private IJSObjectReference? module;
    private IJSObjectReference? mapInstance;

    [Inject]
    private IJSRuntime IJRuntime { get; set; } = default!;

    [Inject]
    private ILogger<Address> Logger { get; set; } = default!;

    [Inject]
    private IGeocoderTranslation GeocoderTranslation { get; set; } = default!;

    [Inject]
    private PDOKLocationService LocationService { get; set; } = default!;

    [CascadingParameter]
    State State { get; set; } = default!;

    [CascadingParameter]
    MainLayout Parent { get; set; } = default!;

    private List<PDOKSuggestion> autoComplete = [];

    string? inputKaasAutoCompleteDing;

    public async Task OnType(ChangeEventArgs e)
    {
        var filter = e.Value?.ToString();

        if (string.IsNullOrEmpty(filter))
        {
            return;
        }

        var addressSuggestion = await LocationService.SuggestAsync(filter, 7);
        if (addressSuggestion.Count != 0)
        {
            autoComplete = addressSuggestion;
            return;
        }
    }

    async Task SelectCustomer(string id)
    {
        var doc = await LocationService.LookupAsync(id);

        inputKaasAutoCompleteDing = doc.weergavenaam;

        autoComplete.Clear();

        var address = await GeocoderTranslation.GetAddressIdAsync(doc.nummeraanduiding_id);

        State.Model.Address = address.Id;
        State.Model.Building = address.BuildingId ?? throw new ArgumentNullException(nameof(address.BuildingId));

        State.DisableNavNext = false;
        Parent.Kaas();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            module = await IJRuntime.InvokeAsync<IJSObjectReference>("import", "./js/mapbox.js");
            mapInstance = await module.InvokeAsync<IJSObjectReference>("addMapToElement", mapElement);
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (mapInstance is not null)
        {
            try
            {
                await mapInstance.DisposeAsync();
            }
            catch (JSDisconnectedException) { }
        }

        if (module is not null)
        {
            try
            {
                await module.DisposeAsync();
            }
            catch (JSDisconnectedException) { }
        }
    }
}
