using FunderMaps.Core.Types;
using FunderMaps.Core.Types.Control;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Inquiry base entity.
/// </summary>
public class InquiryBase<TParent> : IEntityIdentifier<int>
{
    public int Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Client document identifier.
    /// </summary>
    [Required]
    public string DocumentName { get; set; } = default!;

    /// <summary>
    ///     Inspection.
    /// </summary>
    public bool Inspection { get; set; }

    /// <summary>
    ///     Joint measurement.
    /// </summary>
    public bool JointMeasurement { get; set; }

    /// <summary>
    ///     Floor measurement.
    /// </summary>
    public bool FloorMeasurement { get; set; }

    /// <summary>
    ///     Note.
    /// </summary>
    [DataType(DataType.MultilineText)]
    public string? Note { get; set; }

    /// <summary>
    ///     Original document creation.
    /// </summary>
    [DataType(DataType.DateTime)]
    [Required, Range(typeof(DateTime), "01/01/1000", "01/01/2100")]
    public DateTime DocumentDate { get; set; }

    /// <summary>
    ///     Document file name.
    /// </summary>
    [Required]
    public string DocumentFile { get; set; } = default!;

    /// <summary>
    ///     Report type.
    /// </summary>
    public InquiryType Type { get; set; }

    /// <summary>
    ///     Coforms the F3O standaard.
    /// </summary>
    public bool StandardF3o { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing inquiry.</returns>
    public override string ToString() => DocumentName;
}

/// <summary>
///     Inquiry entity.
/// </summary>
public class Inquiry : InquiryBase<Inquiry>
{
}

/// <summary>
///     Inquiry full entity.
/// </summary>
public sealed class InquiryFull : InquiryBase<InquiryFull>
{
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

