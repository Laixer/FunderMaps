using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

// TODO: Maybe this should be a type or a model.
/// <summary>
///     User entity.
/// </summary>
public sealed class OrganizationUser : User
{
    /// <summary>
    ///     User role in organization.
    /// </summary>
    [Required]
    public OrganizationRole OrganizationRole { get; set; }
}
