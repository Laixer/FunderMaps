using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Foundation recovery entity.
/// </summary>
public sealed class Recovery : IEntityIdentifier<int>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Note.
    /// </summary>
    [DataType(DataType.MultilineText)]
    public string? Note { get; set; }

    /// <summary>
    ///     Foundation recovery type.
    /// </summary>
    [Required]
    public RecoveryDocumentType Type { get; set; }

    /// <summary>
    ///     Document file name.
    /// </summary>
    [Required]
    public string DocumentFile { get; set; } = default!;

    /// <summary>
    ///     Document date.
    /// </summary>
    [DataType(DataType.DateTime)]
    [Required, Range(typeof(DateTime), "01/01/1000", "01/01/2100")]
    public DateTime DocumentDate { get; set; }

    /// <summary>
    ///     Client document identifier.
    /// </summary>
    [Required]
    public string DocumentName { get; set; } = default!;

    /// <summary>
    ///     Attribution control.
    /// </summary>
    public AttributionControl Attribution { get; set; } = new();

    /// <summary>
    ///     State control.
    /// </summary>
    public StateControl State { get; set; } = new();

    /// <summary>
    ///     Access control.
    /// </summary>
    public AccessControl Access { get; set; } = new();

    /// <summary>
    ///     Record control.
    /// </summary>
    public RecordControl Record { get; set; } = new();
}
