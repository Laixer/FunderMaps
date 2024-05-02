using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Recovery sample entity.
/// </summary>
public sealed class RecoverySample : RecordControl, IEntityIdentifier<int>
{
    /// <summary>
    ///     Recovery sample identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Project sample identifier.
    /// </summary>
    public int Recovery { get; set; }

    /// <summary>
    ///     Note.
    /// </summary>
    [DataType(DataType.MultilineText)]
    public string? Note { get; set; }

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Required]
    public string Building { get; set; } = default!;

    /// <summary>
    ///     Status.
    /// </summary>
    public RecoveryStatus? Status { get; set; }

    /// <summary>
    ///     Type.
    /// </summary>
    public RecoveryType Type { get; set; }

    /// <summary>
    ///     Pile type.
    /// </summary>
    public PileType? PileType { get; set; }

    /// <summary>
    ///     Contractor organization identifier.
    /// </summary>
    public int? Contractor { get; set; }

    /// <summary>
    ///     Facade.
    /// </summary>
    public Facade[]? Facade { get; set; }

    /// <summary>
    ///     Permit.
    /// </summary>
    public string? Permit { get; set; }

    /// <summary>
    ///     Permit date.
    /// </summary>
    public DateTime? PermitDate { get; set; }

    /// <summary>
    ///     Recovery date.
    /// </summary>
    public DateTime? RecoveryDate { get; set; }
}
