using System.ComponentModel.DataAnnotations;
using FunderMaps.Core.Types;

namespace FunderMaps.AspNetCore.DataTransferObjects;

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
