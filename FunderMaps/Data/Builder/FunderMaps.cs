using Microsoft.EntityFrameworkCore;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

namespace FunderMaps.Data.Builder
{
    public static class FunderMaps
    {
        public static void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_address");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedNever();
                entity.Property(e => e.Street).IsRequired().HasColumnName("address").HasMaxLength(256);
                entity.Property(e => e.AddressNumber).IsRequired().HasColumnName("address_number");
                entity.Property(e => e.AddressNumberPostfix).HasColumnName("address_number_postfix").HasMaxLength(8);
                entity.Property(e => e.City).HasColumnName("city").HasMaxLength(256);
                entity.Property(e => e.Postbox).HasColumnName("postbox").HasMaxLength(8);
                entity.Property(e => e.Zipcode).HasColumnName("zipcode").HasMaxLength(8);
                entity.Property(e => e.State).HasColumnName("state").HasMaxLength(256);
                entity.Property(e => e.Country).HasColumnName("country").HasMaxLength(256);

                entity.ToTable("address", "application");
            });

            modelBuilder.Entity<OrganizationProposal>(entity =>
            {
                entity.HasKey(e => e.Token).HasName("pk_organization_proposal");

                entity.Property(e => e.Token).HasColumnName("token").HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.NormalizedName).HasColumnName("normalized_name").IsRequired().HasMaxLength(256);
                entity.Property(e => e.Email).HasColumnName("email");

                entity.ToTable("organization_proposal", "application");

                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("idx_organization_proposal_normalized_name")
                    .HasFilter("\"normalized_name\" IS NOT NULL");
            });

            modelBuilder.Entity<OrganizationRole>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_organization_role");

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.NormalizedName).HasColumnName("normalized_name").IsRequired().HasMaxLength(256);
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp").IsConcurrencyToken();

                entity.ToTable("organization_role", "application");

                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("idx_organization_role_normalized_name")
                    .HasFilter("\"normalized_name\" IS NOT NULL");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_organization");

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("uuid_generate_v4()");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasColumnName("normalized_name").IsRequired().HasMaxLength(256);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(256);
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.HomeAddressId).HasColumnName("home_address_id");
                entity.Property(e => e.PostalAddressId).HasColumnName("postal_address_id");
                entity.Property(e => e.RegistrationNumber).HasColumnName("registration_number").HasMaxLength(40);
                entity.Property(e => e.IsDefault).HasColumnName("is_default").IsRequired().HasDefaultValue(false);
                entity.Property(e => e.IsValidated).HasColumnName("is_validated").IsRequired().HasDefaultValue(false);
                entity.Property(e => e.BrandingLogo).HasColumnName("branding_logo").HasMaxLength(256);
                entity.Property(e => e.AttestationOrganizationId).HasColumnName("attestation_organization_id");

                entity.ToTable("organization", "application");

                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("idx_organization_normalized_name")
                    .HasFilter("\"normalized_name\" IS NOT NULL");

                entity.HasOne(d => d.HomeAddress)
                   .WithOne()
                   .HasForeignKey<Organization>(s => s.HomeAddressId)
                   .HasPrincipalKey<Address>(c => c.Id)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_address_home_address_id")
                   .IsRequired();

                entity.HasOne(d => d.PostalAddres)
                   .WithOne()
                   .HasForeignKey<Organization>(s => s.PostalAddressId)
                   .HasPrincipalKey<Address>(c => c.Id)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_address_postal_address_id")
                   .IsRequired();
            });

            modelBuilder.Entity<OrganizationUser>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.OrganizationId }).HasName("pk_organization_user");

                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.OrganizationId).HasColumnName("organization_id").IsRequired();
                entity.Property(e => e.OrganizationRoleId).HasColumnName("organization_role_id");

                entity.HasOne(d => d.User)
                   .WithOne()
                   .HasForeignKey<OrganizationUser>(s => s.UserId)
                   .HasPrincipalKey<FunderMapsUser>(c => c.Id)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_organization_user_user_id")
                   .IsRequired();

                entity.HasOne(d => d.Organization)
                   .WithOne()
                   .HasForeignKey<OrganizationUser>(s => s.OrganizationId)
                   .HasPrincipalKey<Organization>(c => c.Id)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_organization_user_organization_id")
                   .IsRequired();

                entity.HasOne(d => d.OrganizationRole)
                   .WithOne()
                   .HasForeignKey<OrganizationUser>(s => s.OrganizationRoleId)
                   .HasPrincipalKey<OrganizationRole>(c => c.Id)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("fk_organization_user_organization_role_id"); ;

                entity.ToTable("organization_user", "application");
            });

            modelBuilder.HasSequence<int>("address_id_seq", "application");
        }
    }
}
