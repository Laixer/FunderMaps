using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.DataTransferObjects;

/// <summary>
///     Change user organization role DTO.
/// </summary>
public sealed record ChangeOrganizationRoleDto
{
    /// <summary>
    ///     Organization role.
    /// </summary>
    [Required]
    public OrganizationRole Role { get; init; }
}
