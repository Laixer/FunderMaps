﻿namespace FunderMaps.Core.Types.Distributions;

/// <summary>
///     Contains a pair of a <see cref="FoundationType"/> and the total amount
///     of buildings having said <see cref="FoundationType"/>.
/// </summary>
public record FoundationTypePair
{
    /// <summary>
    ///     The type of foundation.
    /// </summary>
    public FoundationType FoundationType { get; set; }

    /// <summary>
    ///     The percentage of buildings having this foundation type.
    /// </summary>
    public decimal Percentage { get; set; }
}
