using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Indicent entity.
/// </summary>
public sealed class Incident : RecordControl, IEntityIdentifier<string>
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public string Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Incident]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     Client identifier.
    /// </summary>
    [Required, Range(1, 99)]
    public int ClientId { get; set; }

    /// <summary>
    ///    Client name.
    /// </summary>
    public string ClientName { get; set; } = "FunderMaps";

    /// <summary>
    ///     Foundation type.
    /// </summary>
    [EnumDataType(typeof(FoundationType))]
    public FoundationType? FoundationType { get; set; }

    /// <summary>
    ///     Building chained to another building.
    /// </summary>
    public bool ChainedBuilding { get; set; }

    /// <summary>
    ///     Whether the contact is an owner of the building.
    /// </summary>
    public bool Owner { get; set; }

    /// <summary>
    ///     Whether foundation was recovered or not.
    /// </summary>
    public bool FoundationRecovery { get; set; }

    /// <summary>
    ///     Whether neighbor foundation was recovered or not.
    /// </summary>
    public bool NeighborRecovery { get; set; }

    /// <summary>
    ///     Foundation damage cause.
    /// </summary>
    [EnumDataType(typeof(FoundationDamageCause))]
    public FoundationDamageCause? FoundationDamageCause { get; set; }

    /// <summary>
    ///     Document name.
    /// </summary>
    public string[]? DocumentFile { get; set; }

    /// <summary>
    ///     Note.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    ///     Internal note.
    /// </summary>
    public string? InternalNote { get; set; }

    /// <summary>
    ///     Fouindational damage.
    /// </summary>
    [ArrayEnumDataTypeAttribute(typeof(FoundationDamageCharacteristics))]
    public FoundationDamageCharacteristics[]? FoundationDamageCharacteristics { get; set; }

    /// <summary>
    ///     Environmental damage.
    /// </summary>
    [ArrayEnumDataTypeAttribute(typeof(EnvironmentDamageCharacteristics))]
    public EnvironmentDamageCharacteristics[]? EnvironmentDamageCharacteristics { get; set; }

    /// <summary>
    ///     Contact email.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    ///     Contact name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Contact phone number.
    /// </summary>
    [Phone]
    [StringLength(16)]
    public string? PhoneNumber { get; set; }

    // TODO: Remove this property, replaced by building.
    /// <summary>
    ///     An address identifier.
    /// </summary>
    [Required]
    public string Address { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Required]
    public string Building { get; set; } = default!;

    /// <summary>
    ///     Audit status.
    /// </summary>
    [EnumDataType(typeof(AuditStatus))]
    public AuditStatus AuditStatus { get; set; }

    /// <summary>
    ///     Question type.
    /// </summary>
    [EnumDataType(typeof(IncidentQuestionType))]
    public IncidentQuestionType QuestionType { get; set; }

    /// <summary>
    ///     Meta data.
    /// </summary>
    public object? Meta { get; set; }
}
