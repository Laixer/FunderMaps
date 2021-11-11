namespace FunderMaps.Core.Entities;

/// <summary>
///     Initialize the object to defauls.
/// </summary>
public interface IInitializable<TEntity>
    where TEntity : class
{
    /// <summary>
    ///     Let the object initliaze defaults after the instance was created.
    /// </summary>
    void InitializeDefaults();

    /// <summary>
    ///     Let the object initliaze defaults from another entity of the same type.
    /// </summary>
    void InitializeDefaults(TEntity other);
}
