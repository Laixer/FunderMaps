using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Models;
using FunderMaps.Data.Identity;

namespace FunderMaps.Data
{
    public class FunderMapsDbContext : IdentityDbContext<FunderMapsUser, FunderMapsRole, Guid>
    {
        public DbSet<OrganizationProposal> OrganizationProposals { get; set; }

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
