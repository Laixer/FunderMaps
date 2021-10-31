namespace FunderMaps.Core.Abstractions;

/// <summary>
///     Application service base.
/// </summary>
/// <remarks>
///     The application service base should be the base class to
///     all services in the application.
/// </remarks>
public abstract class AppServiceBase
{
    /// <summary>
    ///     Application context.
    /// </summary>
    public AppContext AppContext { get; set; }
}
