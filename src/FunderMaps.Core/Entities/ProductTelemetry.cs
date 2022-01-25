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
    public string Product { get; set; }

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
    ///     Organization name.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    ///     Product name.
    /// </summary>
    [Required]
    public string Product { get; set; }

    /// <summary>
    ///     Product hit count.
    /// </summary>
    public int Count { get; set; }
}
