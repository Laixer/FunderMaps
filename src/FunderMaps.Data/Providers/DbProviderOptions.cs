namespace FunderMaps.Data.Providers;

/// <summary>
///     Database provider options.
/// </summary>
public class DbProviderOptions
{
    /// <summary>
    ///     Database connection name.
    /// </summary>
    public string? ConnectionStringName { get; set; }

    /// <summary>
    ///     Database connection string.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <summary>
    ///     The client application name.
    /// </summary>
    public string? ApplicationName { get; set; }
}
