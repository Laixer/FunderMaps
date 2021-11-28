namespace FunderMaps.Core.Types;

/// <summary>
///     Recovery type.
/// </summary>
public enum RecoveryType
{
    /// <summary>
    ///     Table.
    /// </summary>
    Table = 0,

    /// <summary>
    ///     Beam on pile.
    /// </summary>
    BeamOnPile = 1,

    /// <summary>
    ///     Pile lowering.
    /// </summary>
    PileLowering = 2,

    /// <summary>
    ///     Pile in wall.
    /// </summary>
    PileInWall = 3,

    /// <summary>
    ///     Injection.
    /// </summary>
    Injection = 4,

    /// <summary>
    ///     Unknown.
    /// </summary>
    Unknown = 5,
}
