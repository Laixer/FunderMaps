using Microsoft.AspNetCore.Components;

namespace FunderMaps.Incident.Components.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private readonly State state = new();

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public ILogger<MainLayout> Logger { get; set; } = default!;

    [CascadingParameter(Name = "AppRouteData")]
    public Microsoft.AspNetCore.Components.RouteData? RouteData { get; set; }

    public void Kaas()
    {
        StateHasChanged();
    }

    public string VendorLogo()
    {
        if (!string.IsNullOrEmpty(state.Vendor))
        {
            string rootpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var image_svg = Path.Combine(rootpath, $"img/logo_{state.Vendor}.svg");
            if (File.Exists(image_svg))
            {
                return $"/img/logo_{state.Vendor}.svg";
            }

            var image_png = Path.Combine(rootpath, $"img/logo_{state.Vendor}.png");
            if (File.Exists(image_png))
            {
                return $"/img/logo_{state.Vendor}.png";
            }

            var image_jpg = Path.Combine(rootpath, $"img/logo_{state.Vendor}.jpg");
            if (File.Exists(image_jpg))
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

            // TODO: This is a hack, we should use a proper DNS name from configuration.
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
                            case "lingewaard":
                                state.Model.ClientId = 25;
                                break;

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

                            case "feedback":
                                state.Model.ClientId = 77;
                                state.Feedback = true;
                                break;
                        }
                    }
                }
            }
        }

        if (RouteData is not null)
        {
            switch (RouteData.PageType.Name)
            {
                case "Index":
                    state.StepId = 0;
                    break;
                case "Address":
                    state.StepId = 1;
                    break;
                case "FoundationDamageCharacteristics":
                    state.StepId = 3;
                    break;
                case "AddressCharacteristics":
                    state.StepId = 4;
                    break;
                case "FeedbackCharacteristics":
                    state.StepId = 4;
                    break;
                case "FoundationDamage":
                    state.StepId = 4;
                    break;
                case "FoundationType":
                    state.StepId = 5;
                    break;
                case "EnvironmentDamageCharacteristics":
                    state.StepId = 6;
                    break;
                case "Upload":
                    state.StepId = 7;
                    break;
                case "Note":
                    state.StepId = 8;
                    break;
                case "Contact":
                    state.StepId = 9;
                    break;
                case "Finish":
                    state.StepId = 10;
                    break;
            }
        }

        if (state.Feedback && state.StepId == 0)
        {
            NavigationManager.NavigateTo("/survey/address", true);
        }

        if (state.Model.Building is null && state.StepId > 1)
        {
            NavigationManager.NavigateTo("/survey/address", true);
        }

        state.DisableNavNext = state.HoldNavNext();
    }
}
