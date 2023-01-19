using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FunderMaps.RazorMaps.Pages;

public class DiagnoseModel : PageModel
{
    public IWebHostEnvironment CurrentEnvironment { get; set; }

    public DiagnoseModel(IWebHostEnvironment env)
    {
        CurrentEnvironment = env;
    }

    public void OnGet()
    {
    }
}
