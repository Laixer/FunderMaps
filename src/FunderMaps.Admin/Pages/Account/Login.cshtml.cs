using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.Admin.Pages.Account;

[Microsoft.AspNetCore.Authorization.AllowAnonymous]
[Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryToken(Order = 1001)]
public class LoginModel : PageModel
{
    private AspNetCore.Services.SignInService _signInService;

    public LoginModel(AspNetCore.Services.SignInService signInService)
    {
        _signInService = signInService;
    }

    public async Task<Microsoft.AspNetCore.Mvc.IActionResult> OnPostAsync(string email, string password)
    {
        try
        {
            var claimsPrincipal = await _signInService.PasswordSignIn2Async(email, password);

            HttpContext.SignInAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Redirect("/");
        }
        catch (System.Exception e)
        {
            // Ignore all exceptions, BECAUSE THATS HOW ITS DONE!!
        }

        return Page();
    }
}
