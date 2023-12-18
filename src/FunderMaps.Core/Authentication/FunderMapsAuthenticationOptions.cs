namespace FunderMaps.Core.Authentication;

public record FunderMapsAuthenticationOptions
{
    public bool CookieAuthentication { get; set; } = true;
}