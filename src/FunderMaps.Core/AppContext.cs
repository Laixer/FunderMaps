using FunderMaps.Core.Entities;

namespace FunderMaps.Core;

/// <summary>
///     Application context to the entire application.
/// </summary>
/// <remarks>
///     The application context carries the full application state per scope. On every scope
///     a new application context is instanciated. The application context is present at all times
///     within a service scope.
/// </remarks>
public record AppContext
{
    /// <summary>
    ///     Notifies when this call is aborted and thus request operations should be cancelled.
    /// </summary>
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    ///     All organizations of which the current user is a member of.
    /// </summary>
    public List<Organization> Organizations { get; set; } = new();

    /// <summary>
    ///     Active organization.
    /// </summary>
    public Organization ActiveOrganization { get; set; } = default!;

    // FUTURE: REMOVE
    /// <summary>
    ///     Tenant identifier.
    /// </summary>
    public Guid TenantId => ActiveOrganization.Identifier;
}
