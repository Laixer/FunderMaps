using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Exceptions;
using FunderMaps.Maps.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace FunderMaps.Maps.Pages;

public partial class Index : ComponentBase, IAsyncDisposable
{
    private ElementReference mapElement;
    private IJSObjectReference? module;
    private IJSObjectReference? mapInstance;
    private Mapset? mapset;

    [Inject]
    private IMapsetRepository MapsetRepository { get; set; } = default!;

    [Inject]
    private IJSRuntime IJRuntime { get; set; } = default!;

    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; } = default!;

    [Parameter]
    public Guid? Sid { get; set; }

    List<Layer> Layers { get; set; } = new List<Layer>
    {
        new Layer
        {
            Name = "Funderingstype",
            Id = "foundation-type-established",
            Fields = new List<Field>
            {
                new Field{ Color = "c75d43", Name = "Houten paal" },
                new Field{ Color = "deb271", Name = "Houten paal met oplanger" },
                new Field{ Color = "6a6c70", Name = "Betonnen paal" },
                new Field{ Color = "ff3333", Name = "Op staal" },
                new Field{ Color = "bdbebf", Name = "Stalen paal" },
                new Field{ Color = "7192de", Name = "Verzwaarde betonpuntpaal" },
                new Field{ Color = "b271de", Name = "Combinatie" },
                new Field{ Color = "ffec33", Name = "Overig" },
                new Field{ Color = "71decc", Name = "Onbekend" },
            }
        },
        new Layer
        {
            Name = "Hersteld",
            Id = "foundation-recovery",
            Fields = new List<Field>
            {
                new Field{ Color = "5cbe55", Name = "Volledig herstel" },
                new Field{ Color = "47baa5", Name = "Partieel herstel" },
                new Field{ Color = "8c4bb6", Name = "Paalkop verlaging" },
                new Field{ Color = "c67e70", Name = "Grondverbetering" },
                new Field{ Color = "5B4AB7", Name = "Onbekend" },
            }
        },
        new Layer
        {
            Name = "Incident",
            Id = "incident-schiedam",
            Fields = new List<Field>
            {
                new Field{ Color = "d90890", Name = "Incident" },
            }
        },
        new Layer
        {
            Name = "Monitoring",
            Id = "monitoring-schiedam",
            Fields = new List<Field>
            {
                new Field{ Color = "1f0bf9", Name = "Monitor" },
            }
        }
    };

    public async Task ToggleVisibility(string layerId)
    {
        if (module is not null && mapInstance is not null)
        {
            var layer = Layers.Find((l) => l.Id == layerId);

            if (layer is not null)
            {
                if (layer.isVisible)
                {
                    layer.isVisible = false;
                    await module.InvokeVoidAsync("setLayerVisibility", mapInstance, layer.Id, layer.isVisible).AsTask();
                }
                else
                {
                    layer.isVisible = true;
                    await module.InvokeVoidAsync("setLayerVisibility", mapInstance, layer.Id, layer.isVisible).AsTask();
                }
            }
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (Sid is null)
            {
                var authState = await authenticationStateTask;
                if (authState.User.Identity.IsAuthenticated)
                {
                    var orgId = "d8c19418-c832-4c91-8993-84b8ed641448";
                    //
                    // claims = authState.User.Claims;
                    mapset = await MapsetRepository.GetByOrganizationIdAsync(Guid.Parse(orgId));
                }
            }
            else
            {
                mapset = await MapsetRepository.GetAsync((Guid)Sid);

                if (mapset is not null)
                {
                    if (!mapset.Public)
                    {
                        mapset = null;
                    }
                }
            }
        }
        catch (EntityNotFoundException) { }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && mapset is not null)
        {
            module = await IJRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Index.razor.js");
            mapInstance = await module.InvokeAsync<IJSObjectReference>("initMap", mapElement, mapset.Style, mapset.Options);
        }
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (mapInstance is not null)
        {
            await mapInstance.DisposeAsync();
        }

        if (module is not null)
        {
            await module.DisposeAsync();
        }
    }
}
