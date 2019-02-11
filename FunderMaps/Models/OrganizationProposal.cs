using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Models
{
    public class OrganizationProposal
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Guid Token { get; set; }

        public OrganizationProposal(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
