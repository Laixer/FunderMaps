using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

// TODO inherit from StateControl?
/// <summary>
///     Foundation recovery entity.
/// </summary>
public sealed class Recovery : IEntityIdentifier<int>
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public int Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Note.
    /// </summary>
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
    [Required]
    public DateTime DocumentDate { get; set; }

    /// <summary>
    ///     Client document identifier.
    /// </summary>
    [Required]
    public string DocumentName { get; set; } = default!;

    /// <summary>
    ///     Attribution control.
    /// </summary>
    public AttributionControl Attribution { get; set; }

    /// <summary>
    ///     State control.
    /// </summary>
    public StateControl State { get; set; }

    /// <summary>
    ///     Access control.
    /// </summary>
    public AccessControl Access { get; set; }

    /// <summary>
    ///     Record control.
    /// </summary>
    public RecordControl Record { get; set; }
}
