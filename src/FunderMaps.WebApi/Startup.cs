using Microsoft.AspNetCore.Mvc;

[assembly: ApiController]
namespace FunderMaps.WebApi;

// TODO: Remove, fix tests.
[Obsolete("This class is obsolete and will be removed in a future version.")]
#pragma warning disable CS9113 // Parameter is unread.
public class Startup(IConfiguration configuration)
{
}
#pragma warning restore CS9113 // Parameter is unread.