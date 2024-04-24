using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     State entity.
/// </summary>
public sealed class State : IEntityIdentifier<string>
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public string Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required, Geocoder]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     State name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///    Is water State.
    /// </summary>
    public bool Water { get; set; }

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;
}
