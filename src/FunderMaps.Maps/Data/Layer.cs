namespace FunderMaps.Maps.Data;

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