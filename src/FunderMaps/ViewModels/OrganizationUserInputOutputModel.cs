using FunderMaps.Core.Entities;
using FunderMaps.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// User with organization role.
    /// </summary>
    public sealed class OrganizationUserInputOutputModel
    {
        /// <summary>
        /// Organization user.
        /// </summary>
        [Required]
        public FunderMapsUser User { get; set; }

        /// <summary>
        /// Organization role.
        /// </summary>
        [Required]
        public OrganizationRole? Role { get; set; }
    }
}
