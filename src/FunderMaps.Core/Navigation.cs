namespace FunderMaps.Core;

/// <summary>
///     Navigation structure.
/// </summary>
public class Navigation
{
    /// <summary>
    ///     Offset in list.
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    ///     Limit of items in list.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    ///     Return all rows.
    /// </summary>
    public static Navigation All { get => new(); }
}
