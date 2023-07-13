namespace FunderMaps.Core.Entities;

/// <summary>
///     Entity identifier.
/// </summary>
public interface IEntityIdentifier<T>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    T Identifier { get; }
}
