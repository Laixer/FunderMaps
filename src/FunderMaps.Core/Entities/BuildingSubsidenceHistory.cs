using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

public sealed class BuildingSubsidenceHistory
{
    /// <summary>
    ///     Building velocity.
    /// </summary>
    [Required]
    public float Velocity { get; set; } = default!;

    /// <summary>
    ///     Mark date.
    /// </summary>
    public DateOnly MarkAt { get; set; }
}
