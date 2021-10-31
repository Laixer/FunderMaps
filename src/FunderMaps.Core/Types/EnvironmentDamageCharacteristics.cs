namespace FunderMaps.Core.Types;

/// <summary>
///     Environment damage characteristics.
/// </summary>
public enum EnvironmentDamageCharacteristics
{
    /// <summary>
    ///     Subsidence.
    /// </summary>
    Subsidence = 0,

    /// <summary>
    ///     Sagging cewer connections.
    /// </summary>
    SaggingSewerConnection = 1,

    /// <summary>
    ///     Sagging cables and pipes.
    /// </summary>
    SaggingCablesPipes = 2,

    /// <summary>
    ///     Flooding.
    /// </summary>
    Flooding = 3,

    /// <summary>
    ///     Foundation damage nearby.
    /// </summary>
    FoundationDamageNearby = 4,

    /// <summary>
    ///     Elevation.
    /// </summary>
    Elevation = 5,

    /// <summary>
    ///     Increasing traffic.
    /// </summary>
    IncreasingTraffic = 6,

    /// <summary>
    ///     Construction nearby.
    /// </summary>
    ConstructionNearby = 7,

    /// <summary>
    ///     Vegetation nearby.
    /// </summary>
    VegetationNearby = 8,

    /// <summary>
    ///     Sewage leakage.
    /// </summary>
    SewageLeakage = 9,

    /// <summary>
    ///     Low ground water.
    /// </summary>
    LowGroundWater = 10,
}
