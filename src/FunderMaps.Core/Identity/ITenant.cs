namespace FunderMaps.Core.Identity;

/// <summary>
///     Tenant identity.
/// </summary>
public interface ITenant
{
    /// <summary>
    ///     Unique tenant identifier.
    /// </summary>
    Guid Id { get; set; }
}
