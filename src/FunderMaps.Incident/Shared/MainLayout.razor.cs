using Microsoft.AspNetCore.Components;
using FunderMaps.Incident.Data;

namespace FunderMaps.Incident.Shared;

public partial class MainLayout : LayoutComponentBase
{
    private State state = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public ILogger<MainLayout> Logger { get; set; } = default!;

    [CascadingParameter(Name = "AppRouteData")]
    public Microsoft.AspNetCore.Components.RouteData? RouteData { get; set; }

    public void Kaas()
    {
        this.StateHasChanged();
    }

    public string VendorLogo()
    {
        if (!string.IsNullOrEmpty(state.Vendor))
        {
            string rootpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot");

            var image_svg = System.IO.Path.Combine(rootpath, $"img/logo_{state.Vendor}.svg");
            if (System.IO.File.Exists(image_svg))
            {
                return $"/img/logo_{state.Vendor}.svg";
            }

            var image_png = System.IO.Path.Combine(rootpath, $"img/logo_{state.Vendor}.png");
            if (System.IO.File.Exists(image_png))
            {
                return $"/img/logo_{state.Vendor}.png";
            }

            var image_jpg = System.IO.Path.Combine(rootpath, $"img/logo_{state.Vendor}.jpg");
            if (System.IO.File.Exists(image_jpg))
            {
                return $"/img/logo_{state.Vendor}.jpg";
            }
        }

        return "/img/logo.png";
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        var absoluteUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (absoluteUri.HostNameType == UriHostNameType.Dns)
        {
            var hostname = absoluteUri.DnsSafeHost;

            if (hostname.EndsWith("fundermaps.com"))
            {
                var nodes = hostname.Split('.');
                if (nodes.Length == 3)
                {
                    if (nodes[0] != "incident")
                    {
                        state.Vendor = nodes[0].ToLower();

                        switch (state.Vendor)
                        {
                            case "lansingerland":
                                state.Model.ClientId = 24;
                                break;

                            case "regiodeal":
                                state.Model.ClientId = 23;
                                break;

                            case "veenweidefryslan":
                                state.Model.ClientId = 22;
                                break;

                            case "schiedam":
                                state.Model.ClientId = 21;
                                break;

                            case "fundermaps":
                                state.Model.ClientId = 20;
                                break;
                        }
                    }
                }
            }
        }

        if (RouteData is not null)
        {
            switch (RouteData.PageType.ToString())
            {
                case "FunderMaps.Incident.Pages.Index":
                    state.StepId = 0;
                    break;
                case "FunderMaps.Incident.Pages.Address":
                    state.StepId = 1;
                    break;
                case "FunderMaps.Incident.Pages.FoundationDamage":
                    state.StepId = 2;
                    break;
                case "FunderMaps.Incident.Pages.FoundationDamageCharacteristics":
                    state.StepId = 3;
                    break;
                case "FunderMaps.Incident.Pages.AddressCharacteristics":
                    state.StepId = 4;
                    break;
                case "FunderMaps.Incident.Pages.FoundationType":
                    state.StepId = 5;
                    break;
                case "FunderMaps.Incident.Pages.EnvironmentDamageCharacteristics":
                    state.StepId = 6;
                    break;
                case "FunderMaps.Incident.Pages.Upload":
                    state.StepId = 7;
                    break;
                case "FunderMaps.Incident.Pages.Note":
                    state.StepId = 8;
                    break;
                case "FunderMaps.Incident.Pages.Contact":
                    state.StepId = 9;
                    break;
                case "FunderMaps.Incident.Pages.Finish":
                    state.StepId = 10;
                    break;
            }
        }

        state.DisableNavNext = state.HoldNavNext();
    }
}
