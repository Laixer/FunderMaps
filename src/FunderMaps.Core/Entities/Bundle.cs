using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Bundle entity.
/// </summary>
public sealed class Bundle
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public string Id => Tileset;

    /// <summary>
    ///     Tileset name.
    /// </summary>
    public string Tileset { get; set; } = default!;

    /// <summary>
    ///     Whether the bundle is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    ///     Whether the bundle should create a map/tileset.
    /// </summary>
    public bool MapEnabled { get; set; }

    /// <summary>
    ///     Last build date.
    /// </summary>
    public DateTime? BuiltDate { get; set; }

    /// <summary>
    ///     Gets or sets the name for the bundle.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Get the precondition for the bundle.
    /// </summary>
    [Required]
    public string? Precondition { get; set; }

    /// <summary>
    ///     Minimum zoom level.
    /// </summary>
    public int MinZoomLevel { get; set; }

    /// <summary>
    ///     Maximum zoom level.
    /// </summary>
    public int MaxZoomLevel { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing contractor.</returns>
    public override string ToString() => Name;
}
