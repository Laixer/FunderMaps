using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Data;
using FunderMaps.Incident.Shared;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Incident.Pages;

public partial class Address : ComponentBase, IAsyncDisposable
{
    private ElementReference mapElement;
    private IJSObjectReference? module;
    private IJSObjectReference? mapInstance;
    private HttpClient http = new();

    [Inject]
    private IJSRuntime IJRuntime { get; set; } = default!;

    [Inject]
    private ILogger<Address> Logger { get; set; } = default!;

    [Inject]
    private IGeocoderTranslation GeocoderTranslation { get; set; } = default!;

    [CascadingParameter]
    State State { get; set; } = default!;

    [CascadingParameter]
    MainLayout Parent { get; set; } = default!;

    private List<PDOKAddressSuggestion> autoComplete = new();

    string? inputKaasAutoCompleteDing;

    public Address()
    {
        http.BaseAddress = new("https://geodata.nationaalgeoregister.nl/locatieserver/v3/");
    }

    class PDOKAddressSuggestion
    {
        public string id { get; set; }
        public string type { get; set; }
        public float score { get; set; }
        public string weergavenaam { get; set; }
    }

    class PDOKResponse
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public float maxScore { get; set; }
        public List<PDOKAddressSuggestion> docs { get; set; }
    }

    class PDOKResult
    {
        public PDOKResponse response { get; set; }
        // object highlighting;
        // object spellcheck;
    }

    class PDOKLookup
    {
        public string centroide_ll { get; set; }
        public string nummeraanduiding_id { get; set; }
    }

    class PDOKResponse2
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public float maxScore { get; set; }
        public List<PDOKLookup> docs { get; set; }
    }

    class PDOKResult2
    {
        public PDOKResponse2 response { get; set; }
        // object highlighting;
        // object spellcheck;
    }

    public async Task OnType(ChangeEventArgs e)
    {
        var filter = e.Value?.ToString();

        var addressSuggestion = await http.GetFromJsonAsync<PDOKResult>($"suggest?fq=type:adres&q={filter}&rows=7");
        if (addressSuggestion is not null)
        {
            autoComplete = addressSuggestion.response.docs;
        }
    }

    async Task SelectCustomer(string id)
    {
        var suggestion = autoComplete.Find(x => x.id == id);
        if (suggestion is not null)
        {
            inputKaasAutoCompleteDing = suggestion.weergavenaam;
        }

        var customers = await http.GetFromJsonAsync<PDOKResult2>($"lookup?fl=nummeraanduiding_id,centroide_ll&id={id}");
        if (customers is not null)
        {
            var address = await GeocoderTranslation.GetAddressIdAsync(customers.response.docs[0].nummeraanduiding_id);

            State.Model.Address = address.Id;
            State.Model.Building = address.BuildingId ?? throw new ArgumentNullException(nameof(address.BuildingId));

            State.DisableNavNext = false;
            Parent.Kaas();
        }

        autoComplete.Clear();
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
