using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.RazorMaps.Pages;

public class IndexModel : PageModel
{
    private readonly IMapsetRepository _mapsetRepository;
    private readonly Core.AppContext _appContext;

    public List<Mapset> mapSetList = new();

    public List<Layer> Layers { get; set; } = new List<Layer>
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
                new Field{ Color = "ff3333", Name = "Niet onderheid" },
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
            Id = "incident",
            Fields = new List<Field>
            {
                new Field{ Color = "d90890", Name = "Incident" },
            }
        },
        new Layer
        {
            Name = "Monitoring",
            Id = "monitoring",
            Fields = new List<Field>
            {
                new Field{ Color = "4969B8", Name = "Monitoring" },
            }
        },
        new Layer
        {
            Name = "Droogstand",
            Id = "drystand-risk",
            Fields = new List<Field>
            {
                new Field{ Color = "42FF33", Name = "A" },
                new Field{ Color = "D1FF33", Name = "B" },
                new Field{ Color = "FFEC33", Name = "C" },
                new Field{ Color = "FFAC33", Name = "D" },
                new Field{ Color = "FF5533", Name = "E" },
            }
        },
        new Layer
        {
            Name = "Ontwateringsdiepte",
            Id = "dewatering-depth-risk",
            Fields = new List<Field>
            {
                new Field{ Color = "42FF33", Name = "A" },
                new Field{ Color = "D1FF33", Name = "B" },
                new Field{ Color = "FFEC33", Name = "C" },
                new Field{ Color = "FFAC33", Name = "D" },
                new Field{ Color = "FF5533", Name = "E" },
            }
        },
        new Layer
        {
            Name = "Bacteriele aantasting",
            Id = "bio-infection-risk",
            Fields = new List<Field>
            {
                new Field{ Color = "42FF33", Name = "A" },
                new Field{ Color = "D1FF33", Name = "B" },
                new Field{ Color = "FFEC33", Name = "C" },
                new Field{ Color = "FFAC33", Name = "D" },
                new Field{ Color = "FF5533", Name = "E" },
            }
        },
        new Layer
        {
            Name = "Overige risicos",
            Id = "unclassified-risk",
            Fields = new List<Field>
            {
                new Field{ Color = "42FF33", Name = "A" },
                new Field{ Color = "D1FF33", Name = "B" },
                new Field{ Color = "FFEC33", Name = "C" },
                new Field{ Color = "FFAC33", Name = "D" },
                new Field{ Color = "FF5533", Name = "E" },
            }
        },
        new Layer
        {
            Name = "Pandzakking",
            Id = "velocity",
            Fields = new List<Field>
            {
                new Field{ Color = "f7fbff", Name = "> 0 mm/jaar" },
                new Field{ Color = "d8e7f5", Name = "0,0 t/m -0,5 mm/jaar" },
                new Field{ Color = "b0d2e8", Name = "-0,5 t/m -1,0 mm/jaar" },
                new Field{ Color = "73b3d8", Name = "-1,0 t/m -1,5 mm/jaar" },
                new Field{ Color = "3e8ec4", Name = "-1,5 t/m -2,0 mm/jaar" },
                new Field{ Color = "1563aa", Name = "-2,0 t/m -2,5 mm/jaar" },
                new Field{ Color = "08306b", Name = "< -2,5 mm/jaar" },
            }
        },
        new Layer
        {
            Name = "Funderingstype indicatief",
            Id = "foundation-type-indicative",
            Fields = new List<Field>
            {
                new Field{ Color = "d8907d", Name = "Houten paal" },
                new Field{ Color = "edd5b1", Name = "Houten paal met oplanger" },
                new Field{ Color = "6a6c70", Name = "Betonnen paal" },
                new Field{ Color = "ff8080", Name = "Niet onderheid" },
            }
        },
        new Layer
        {
            Name = "Eigenaar",
            Id = "owner",
            Fields = new List<Field>
            {
                new Field{ Color = "d11313", Name = "Eigenaar" },
            }
        },
        new Layer
        {
            Name = "Bouwjaar",
            Id = "construction-year",
            Fields = new List<Field>
            {
                new Field{ Color = "293575", Name = "< 1960" },
                new Field{ Color = "1261A3", Name = "1960 t/m 1970" },
                new Field{ Color = "69A8DE", Name = "1970 t/m 1980" },
                new Field{ Color = "99C1E9", Name = "1980 t/m 1990" },
                new Field{ Color = "B378B1", Name = "1990 t/m 2000" },
                new Field{ Color = "bd6495", Name = "2000 t/m 2010" },
                new Field{ Color = "bd6495", Name = "2010 t/m 2020" },
                new Field{ Color = "d11313", Name = "> 2020" },
            }
        },
        new Layer
        {
            Name = "Handhavingstermijn (jaar)",
            Id = "enforcement-term",
            Fields = new List<Field>
            {
                new Field{ Color = "64DEBC", Name = "> 25" },
                new Field{ Color = "55E293", Name = "20 t/m 25" },
                new Field{ Color = "46E65F", Name = "15 t/m 20" },
                new Field{ Color = "4CEB36", Name = "10 t/m 15" },
                new Field{ Color = "77F025", Name = "5 t/m 10" },
                new Field{ Color = "AEF614", Name = "0 t/m 5" },
                new Field{ Color = "D0E218", Name = "0 t/m -5" },
                new Field{ Color = "CEB31B", Name = "-5 t/m -10" },
                new Field{ Color = "CEB31B", Name = "-10 t/m -15" },
                new Field{ Color = "A85520", Name = "-15 t/m -20" },
                new Field{ Color = "973321", Name = "-20 t/m -25" },
                new Field{ Color = "86222A", Name = "< -25" },
            }
        },
        new Layer
        {
            Name = "Rapportage type",
            Id = "inquiry-type",
            Fields = new List<Field>
            {
                new Field{ Color = "B54CB0", Name = "Monitoring" },
                new Field{ Color = "8C4BB6", Name = "Notitie" },
                new Field{ Color = "5B4AB7", Name = "Snelle scan" },
                new Field{ Color = "4969B8", Name = "Sloop onderzoek" },
                new Field{ Color = "489BB9", Name = "Second opinion" },
                new Field{ Color = "47BAA5", Name = "Archief onderzoek" },
                new Field{ Color = "4EBC77", Name = "Bouwkundig onderzoek" },
                new Field{ Color = "5CBE55", Name = "Funderingsadvies" },
                new Field{ Color = "BDC262", Name = "Funderingsonderzoek" },
                new Field{ Color = "C4A169", Name = "Extra onderzoek" },
                new Field{ Color = "C67E70", Name = "Grondwaterniveau onderzoek" },
                new Field{ Color = "6A6C70", Name = "Onbekend" },
            }
        },
        new Layer
        {
            Name = "Schadeoorzaak",
            Id = "damage-cause",
            Fields = new List<Field>
            {
                new Field{ Color = "55B5A7", Name = "Ontwateringsdiepte" },
                new Field{ Color = "4B8FBF", Name = "Overbelasting" },
                new Field{ Color = "4145C9", Name = "Bacteriële aantasting" },
                new Field{ Color = "8936D4", Name = "Schimmelaantasting" },
                new Field{ Color = "DE2CCF", Name = "Bodemdaling" },
                new Field{ Color = "D2386F", Name = "Planten en wortels" },
                new Field{ Color = "C75D43", Name = "Aardbeving" },
                new Field{ Color = "BBA14F", Name = "Partieel funderingsherstel" },
                new Field{ Color = "95B05A", Name = "Constructiefout" },
                new Field{ Color = "6EA466", Name = "Negatieve kleef" },
                new Field{ Color = "6A6C70", Name = "Onbekend" },
            }
        }
    };

    public IndexModel(IMapsetRepository mapsetRepository, Core.AppContext appContext)
    {
        _mapsetRepository = mapsetRepository;
        _appContext = appContext;
    }

    public async Task OnGetAsync(Guid mapId)
    {
        if (mapId != Guid.Empty)
        {
            var set = await _mapsetRepository.GetPublicAsync(mapId);
            if (set.Layers is not null && set.Layers.Length > 0)
            {
                set.LayerNavigation = Layers.Where((l) => set.Layers.Contains(l.Id)).ToList();
            }
            mapSetList.Add(set);
        }

        if (!mapSetList.Any())
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme);
            }
        }

        foreach (var organization in _appContext.Organizations)
        {
            await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync(organization.Id))
            {
                if (set.Layers is not null && set.Layers.Length > 0)
                {
                    set.LayerNavigation = Layers.Where((l) => set.Layers.Contains(l.Id)).ToList();
                }
                mapSetList.Add(set);
            }
        }
    }
}
