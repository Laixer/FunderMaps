using FunderMaps.Core.Entities;
using System.Net;
using System.Reflection;
using System.Security.Principal;

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
    ///     User identity.
    /// </summary>
    public User User { get; set; } = default!;

    /// <summary>
    ///     All organizations of which the current user is a member of.
    /// </summary>
    public List<Organization> Organizations { get; set; } = new();

    /// <summary>
    ///     Active organization.
    /// </summary>
    public Organization ActiveOrganization { get; set; } = default!;

    /// <summary>
    ///     User identifier.
    /// </summary>
    public Guid UserId => User.Identifier;

    // FUTURE: REMOVE
    /// <summary>
    ///     Tenant identifier.
    /// </summary>
    public Guid TenantId => ActiveOrganization.Identifier;

    /// <summary>
    ///     Active organization identifier.
    /// </summary>
    public Guid OrganizationId => ActiveOrganization.Identifier;

    /// <summary>
    ///     Gets or sets the IP address of the remote target. Can be null.
    /// </summary>
    public IPAddress? RemoteIpAddress { get; set; }

    /// <summary>
    ///     Gets or sets the Host header. May include the port.
    /// </summary>
    public string Host { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the user agent. Can be null.
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    ///     Identity object.
    /// </summary>
    public IIdentity? Identity { get; set; }

    /// <summary>
    ///     Absolute path to the application directory.
    /// </summary>
    public static string? ApplicationDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}
