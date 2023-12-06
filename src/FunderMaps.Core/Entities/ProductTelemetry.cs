using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Product telemetry.
/// </summary>
public sealed class ProductTelemetry
{
    /// <summary>
    ///     Product name.
    /// </summary>
    [Required]
    public string Product { get; set; } = default!;

    /// <summary>
    ///     Product hit count.
    /// </summary>
    public int Count { get; set; }
}

// TODO: Consolidate
/// <summary>
///     Product telemetry.
/// </summary>
public sealed class ProductTelemetry2
{
    /// <summary>
    ///     Organization ID.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    ///     Organization name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Product name.
    /// </summary>
    [Required]
    public string Product { get; set; } = default!;

    /// <summary>
    ///     Product hit count.
    /// </summary>
    public int Count { get; set; }
}

