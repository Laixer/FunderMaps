using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Organization entity.
/// </summary>
public sealed class Organization : IEntityIdentifier<Guid>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the name for the organization.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the email address for the organization.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    ///     Geospatial account area.
    /// </summary>
    public SpatialBox Area { get; set; } = default!;

    /// <summary>
    ///     Geospatial account area center.
    /// </summary>
    public SpatialPoint Center { get; set; } = default!;

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing organization.</returns>
    public override string ToString() => Name;
}
