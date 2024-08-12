namespace FunderMaps.Core.Entities;

public sealed class UserData
{
    /// <summary>
    ///    Metadata.
    /// </summary>
    public object? Metadata { get; set; }

    /// <summary>
    ///     Record last update.
    /// </summary>
    public DateTime? UpdateDate { get; set; }
}
