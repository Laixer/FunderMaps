namespace FunderMaps.Core.Types.Control;

// TODO: Should be a record
/// <summary>
///     Attribution represents an entity partition for user and organizational relations.
/// </summary>
public class AttributionControl
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Reviewer idenitfier.
    /// </summary>
    public Guid Reviewer { get; set; }

    // TODO: Cannot be null
    /// <summary>
    ///    Reviewer name.
    /// </summary>
    public string? ReviewerName { get; set; }

    /// <summary>
    ///     Creator identifier.
    /// </summary>
    public Guid Creator { get; set; }

    // TODO: Cannot be null
    /// <summary>
    ///    Creator name.
    /// </summary>
    public string? CreatorName { get; set; }

    /// <summary>
    ///     Owner identifier.
    /// </summary>
    public Guid Owner { get; set; }

    // TODO: Cannot be null
    /// <summary>
    ///     Owner name.
    /// </summary>
    public string? OwnerName { get; set; }

    /// <summary>
    ///     Contractor identifier.
    /// </summary>
    public int Contractor { get; set; }

    // TODO: Cannot be null
    /// <summary>
    ///    Contractor name.
    /// </summary>
    public string? ContractorName { get; set; }
}
