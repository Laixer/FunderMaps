using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

namespace FunderMaps.Data
{
    public class FunderMapsDbContext : IdentityDbContext<FunderMapsUser, FunderMapsRole, Guid>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationProposal> OrganizationProposals { get; set; }
        public DbSet<OrganizationUser> OrganizationUsers { get; set; }

        public FunderMapsDbContext(DbContextOptions<FunderMapsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Builder.Identity.ModelCreating(modelBuilder);
            Builder.FunderMaps.ModelCreating(modelBuilder);
        }
    }
}
