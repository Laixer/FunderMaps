using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Models
{
    public class OrganizationProposal
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public Guid Token { get; set; } = Guid.NewGuid();

        public OrganizationProposal(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
