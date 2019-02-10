using Microsoft.EntityFrameworkCore;
using FunderMaps.Models;

namespace FunderMaps.Data.Builder
{
    public static class FunderMaps
    {
        public static void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationProposal>(entity =>
            {
                entity.HasKey(e => e.Name).HasName("pk_organization_proposal");

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Token).HasColumnName("value");

                entity.HasIndex(e => e.Token).IsUnique().HasName("idx_organization_proposal_value")
                    .HasFilter("\"value\" IS NOT NULL");

                entity.ToTable("organization_proposal", "application");
            });
        }
    }
}
