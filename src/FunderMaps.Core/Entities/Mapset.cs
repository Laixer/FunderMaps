using System.ComponentModel.DataAnnotations;

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
public sealed class Mapset : IEntityIdentifier<Guid>
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public Guid Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Map style.
    /// </summary>
    [Required]
    public string Style { get; set; } = default!;

    /// <summary>
    ///     Map layers.
    /// </summary>
    public string[]? Layers { get; set; }

    /// <summary>
    ///     Map options.
    /// </summary>
    public object? Options { get; set; }

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
    public object? LayerSet { get; set; }

    // TODO: Has been replaced by LayerSet
    /// <summary>
    ///     List of layers in this mapset.
    /// </summary>
    public List<Layer> LayerNavigation { get; set; } = [];
}
