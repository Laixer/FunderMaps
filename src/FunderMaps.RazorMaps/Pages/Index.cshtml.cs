using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.RazorMaps.Pages;

public class IndexModel : PageModel
{
    public Guid MapId { get; set; } = Guid.Empty;

    public async Task OnGetAsync(Guid mapId)
    {
        MapId = mapId;
        if (mapId == Guid.Empty)
        {
            if (User.Identity is null || !User.Identity.IsAuthenticated)
            {
                await HttpContext.ChallengeAsync();
            }
        }
    }
}
