namespace FunderMaps.Core.Types.Distributions;

// FUTURE: Make pair a generic type.
/// <summary>
///     Represents how many buildings have their construction years in a given
/// <see cref="Decade"/>.
/// </summary>
public sealed class ConstructionYearPair
{
    /// <summary>
    ///     Decade that represents this construction year pair.
    /// </summary>
    public Years Decade { get; set; }

    /// <summary>
    ///     Total amount of items that fall into this decade.
    /// </summary>
    public int TotalCount { get; set; }
}
