namespace TdmClient;

/// <summary>
/// TDM service configuration.
/// </summary>
public sealed class TdmConfig
{
    /// <summary>
    /// Whether or not to use the acceptance service endpoint.
    /// </summary>
    public bool UseAcceptanceMode { get; set; } = false;

    /// <summary>
    /// Consumer key.
    /// </summary>
    public string ConsumerKey { get; set; }

    /// <summary>
    /// Consumer secret.
    /// </summary>
    public string ConsumerSecret { get; set; }
}
