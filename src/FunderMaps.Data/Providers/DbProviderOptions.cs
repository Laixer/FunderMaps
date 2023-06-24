namespace FunderMaps.Data.Providers;

/// <summary>
///     Database provider options.
/// </summary>
internal class DbProviderOptions
{
    /// <summary>
    ///     Database connection name.
    /// </summary>
    public string? ConnectionStringName { get; set; }

    /// <summary>
    ///     The client application name.
    /// </summary>
    public string? ApplicationName { get; set; }
}
