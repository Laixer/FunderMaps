using System.Security.Claims;
using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.RazorMaps.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    private readonly SignInService _signInService;

    public LoginModel(SignInService signInService)
    {
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        ClaimsPrincipal principal = await _signInService.PasswordSignIn3Async(Username, Password);

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);

        return RedirectToPage("/Index");
    }
}