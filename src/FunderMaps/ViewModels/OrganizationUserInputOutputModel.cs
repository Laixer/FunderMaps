using System.ComponentModel.DataAnnotations;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

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
        public OrganizationRole Role { get; set; }
    }
}
