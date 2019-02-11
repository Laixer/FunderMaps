using System.ComponentModel.DataAnnotations;
using FunderMaps.Data.Identity;

namespace FunderMaps.Models
{
    public class OrganizationInitiation
    {
        [Required]
        public FunderMapsUser User { get; set; }

        [Required]
        public Organization Organization { get; set; }
    }
}
