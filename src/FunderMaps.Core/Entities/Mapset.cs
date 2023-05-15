using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.Core.Entities;

public class Layer
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool isVisible { get; set; } = true;
    public List<Field> Fields { get; set; } = new();
}

public class Field
{
    public string? Color { get; set; }
    public string? Name { get; set; }
}

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

    /// <summary>
    ///     Consent text.
    /// </summary>
    public string? Consent { get; set; }

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public string? FenceMunicipality { get; set; }

    /// <summary>
    ///     Map layers sets.
    /// </summary>
    public object LayerSet { get; set; }

    /// <summary>
    ///     List of layers in this mapset.
    /// </summary>
    public List<Layer> LayerNavigation { get; set; } = new();
}
