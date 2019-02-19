using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FunderMaps.Models.Identity;

namespace FunderMaps.Data.Builder
{
    public static class Identity
    {
        public static void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FunderMapsRole>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_role");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp").IsConcurrencyToken();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(256);
                entity.Property(e => e.NormalizedName).HasColumnName("normalized_name").HasMaxLength(256);

                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("idx_role_normalized_name")
                    .HasFilter("\"normalized_name\" IS NOT NULL");

                entity.ToTable("role", "application");

                entity.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                entity.HasMany<IdentityRoleClaim<Guid>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_role_claim");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimType).HasColumnName("claim_type");
                entity.Property(e => e.ClaimValue).HasColumnName("claim_value");
                entity.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();

                entity.HasIndex(e => e.RoleId).HasName("idx_role_claim_role_id");

                entity.ToTable("role_claim", "application");
            });

            modelBuilder.Entity<FunderMapsUser>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_user");


                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.GivenName).HasColumnName("given_name").HasMaxLength(256);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(256);
                entity.Property(e => e.UserName).HasColumnName("username").HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasColumnName("normalized_username").HasMaxLength(256);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasColumnName("normalized_email").IsRequired().HasMaxLength(256);
                entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed").HasDefaultValue(false);
                entity.Property(e => e.Avatar).HasColumnName("avatar").HasMaxLength(256);
                entity.Property(e => e.JobTitle).HasColumnName("job_title");
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
                entity.Property(e => e.SecurityStamp).HasColumnName("security_stamp");
                entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed").HasDefaultValue(false);
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("two_factor_enabled").HasDefaultValue(false);
                entity.Property(e => e.LockoutEnd).HasColumnName("lockout_end");
                entity.Property(e => e.LockoutEnabled).HasColumnName("lockout_enabled").HasDefaultValue(false);
                entity.Property(e => e.AccessFailedCount).HasColumnName("access_failed_count");
                entity.Property(e => e.AttestationPrincipalId).HasColumnName("attestation_principal_id");

                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp").IsConcurrencyToken();

                entity.HasIndex(e => e.NormalizedUserName).HasName("idx_user_normalized_username")
                    .IsUnique().HasFilter("\"normalized_username\" IS NOT NULL");

                entity.HasIndex(e => e.NormalizedEmail).HasName("idx_user_normalized_email")
                    .IsUnique().HasFilter("\"normalized_email\" IS NOT NULL");

                entity.ToTable("user", "application");

                entity.HasMany<IdentityUserClaim<Guid>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                entity.HasMany<IdentityUserLogin<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                entity.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                entity.HasMany<IdentityUserRole<Guid>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("pk_user_claim");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ClaimType).HasColumnName("claim_type");
                entity.Property(e => e.ClaimValue).HasColumnName("claim_value");
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

                entity.HasIndex(e => e.UserId).HasName("idx_user_claim_user_id");

                entity.ToTable("user_claim", "application");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey }).HasName("pk_user_login");

                entity.Property(e => e.LoginProvider).HasColumnName("login_provider").HasMaxLength(128);
                entity.Property(e => e.ProviderKey).HasColumnName("provider_key").HasMaxLength(128);
                entity.Property(e => e.ProviderDisplayName).HasColumnName("provider_display_name");
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

                entity.HasIndex(e => e.UserId).HasName("idx_user_login_user_id");

                entity.ToTable("user_login", "application");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("pk_user_role");

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasIndex(e => e.RoleId).HasName("idx_user_role_role_id");

                entity.ToTable("user_role", "application");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }).HasName("pk_user_token");

                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.LoginProvider).HasColumnName("login_provider").HasMaxLength(128);
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(128);
                entity.Property(e => e.Value).HasColumnName("valuee");

                entity.ToTable("user_token", "application");
            });
        }
    }
}
