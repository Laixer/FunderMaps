using FunderMaps.Core.Identity;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Organization entity.
/// </summary>
public sealed class Organization : IEntityIdentifier<Guid>, ITenant
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public Guid Identifier => Id;

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
    public SpatialBox Area { get; set; }

    /// <summary>
    ///     Geospatial account area center.
    /// </summary>
    public SpatialPoint Center { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing organization.</returns>
    public override string ToString() => Name;
}
