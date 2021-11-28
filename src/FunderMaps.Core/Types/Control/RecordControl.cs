namespace FunderMaps.Core.Types.Control;

/// <summary>
///     Record timestamps.
/// </summary>
public class RecordControl
{
    /// <summary>
    ///     Record create date.
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    ///     Record last update.
    /// </summary>
    public DateTime? UpdateDate { get; set; }

    /// <summary>
    ///     Record delete date.
    /// </summary>
    public DateTime? DeleteDate { get; set; }
}
