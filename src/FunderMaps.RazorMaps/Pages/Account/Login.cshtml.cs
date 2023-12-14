using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.RazorMaps.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    private readonly SignInService _signInService;

    public LoginModel(SignInService signInService)
        => _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));

    public async Task<IActionResult> OnPostAsync()
    {
        if (Username is null || Password is null)
        {
            return Page();
        }

        var principal = await _signInService.PasswordSignInAsync(Username, Password, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        return RedirectToPage("/Index");
    }
}