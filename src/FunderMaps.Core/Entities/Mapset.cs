using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Mapset entity.
/// </summary>
public sealed class Mapset : IdentifiableEntity<Mapset, Guid>
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public Mapset()
        : base(e => e.Id)
    {
    }

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Incident]
    public Guid Id { get; set; }

    /// <summary>
    ///     Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Map style.
    /// </summary>
    public string Style { get; set; }

    /// <summary>
    ///     Map layers.
    /// </summary>
    public string[] Layers { get; set; }

    /// <summary>
    ///     Map options.
    /// </summary>
    public object Options { get; set; }

    /// <summary>
    ///     If the map is public or not.
    /// </summary>
    public bool Public { get; set; }
}
