using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.Admin.Pages.Account;

[Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryToken(Order = 1001)]
public class LogoutModel : PageModel
{
    public async Task<Microsoft.AspNetCore.Mvc.IActionResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync();

        return Redirect("/account/login");
    }
}
