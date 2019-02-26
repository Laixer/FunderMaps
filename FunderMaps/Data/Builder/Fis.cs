using Microsoft.EntityFrameworkCore;
using FunderMaps.Data.Converters;
using FunderMaps.Models.Fis;

namespace FunderMaps.Data.Builder
{
    public static class Fis
    {
        public static void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlHasEnum("report", "base_level", new[] { "NAP (NL)", "TAW (BE)", "NN (DE)" })
                .ForNpgsqlHasEnum("report", "enforcement_term", new[] { "0-5", "5-10", "10-20", "5", "10", "15", "20", "25", "30" });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => new { e.StreetName, e.BuildingNumber, e.BuildingNumberSuffix });

                entity.Property(e => e.StreetName).HasColumnName("street_name").HasMaxLength(128);
                entity.Property(e => e.BuildingNumber).HasColumnName("building_number");
                entity.Property(e => e.BuildingNumberSuffix).HasColumnName("building_number_suffix").HasColumnType("character(2)");
                entity.Property(e => e.District).HasColumnName("district").HasMaxLength(64);
                entity.Property(e => e.Neighborhood).HasColumnName("neighborhood").HasMaxLength(64);
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.Subneighborhood).HasColumnName("subneighborhood").HasMaxLength(64);
                entity.Property(e => e.Township).HasColumnName("township").HasMaxLength(64);

                entity.ToTable("address", "report");
            });

            modelBuilder.Entity<FoundationDamageCause>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);

                entity.ToTable("foundation_damage_cause", "report");
            });

            modelBuilder.Entity<FoundationQuality>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);

                entity.ToTable("foundation_quality", "report");
            });

            modelBuilder.Entity<FoundationRecovery>(entity =>
            {
                entity.HasKey(e => new { e.BuildingNumber, e.StreetName, e.BuildingNumberSuffix });

                entity.ToTable("foundation_recovery", "report");

                entity.Property(e => e.BuildingNumber).HasColumnName("building_number");
                entity.Property(e => e.StreetName).HasColumnName("street_name").HasMaxLength(128);
                entity.Property(e => e.BuildingNumberSuffix).HasColumnName("building_number_suffix").HasColumnType("character(2)");
                entity.Property(e => e.ContractorName).HasColumnName("contractor_name").HasMaxLength(96);
                entity.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("time with time zone");
                entity.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("time with time zone");
                entity.Property(e => e.Evidence).IsRequired().HasColumnName("evidence").HasMaxLength(32);
                entity.Property(e => e.EvidenceDocument).IsRequired().HasColumnName("evidence_document").HasColumnType("character varying(256)[]");
                entity.Property(e => e.EvidenceName).IsRequired().HasColumnName("evidence_name").HasMaxLength(96);
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(32);
                entity.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("time with time zone");
                entity.Property(e => e.Year).HasColumnName("year").HasColumnType("report.year");

                entity.HasOne(d => d.EvidenceNavigation)
                    .WithMany(p => p.FoundationRecovery)
                    .HasForeignKey(d => d.Evidence)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("foundation_recovery_evidence_fkey");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.FoundationRecovery)
                    .HasForeignKey(d => d.Type)
                    .HasConstraintName("foundation_recovery_type_fkey");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.FoundationRecovery)
                    .HasForeignKey(d => new { d.StreetName, d.BuildingNumber, d.BuildingNumberSuffix })
                    .HasConstraintName("foundation_recovery_street_name_fkey");
            });

            modelBuilder.Entity<FoundationRecoveryEvidence>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);

                entity.ToTable("foundation_recovery_evidence", "report");
            });

            modelBuilder.Entity<FoundationRecoveryType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);

                entity.ToTable("foundation_recovery_type", "report");
            });

            modelBuilder.Entity<FoundationType>(entity =>
            {
                entity.ToTable("foundation_type", "report");

                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organization", "attestation");

                entity.HasIndex(e => e.Name)
                    .HasName("organization_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("nextval('attestation.organization_id_seq'::regclass)");
                entity.Property(e => e.Name).IsRequired().HasColumnName("name").HasMaxLength(32);
            });

            modelBuilder.Entity<Principal>(entity =>
            {
                entity.ToTable("principal", "attestation");

                entity.HasIndex(e => e.Email)
                    .HasName("principal_email_key");

                entity.HasIndex(e => e.NickName)
                    .HasName("principal_nick_name_key")
                    .IsUnique();

                entity.HasIndex(e => new { e.FirstName, e.MiddleName, e.LastName })
                    .HasName("principal_first_name_middle_name_last_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("nextval('attestation.principal_id_seq'::regclass)");
                entity.Property(e => e.Email).IsRequired().HasColumnName("email").HasMaxLength(256);
                entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(64);
                entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(96);
                entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(32);
                entity.Property(e => e.NickName).IsRequired().HasColumnName("nick_name").HasMaxLength(32);
                entity.Property(e => e.Organization).HasColumnName("organization");

                entity.HasOne(d => d.OrganizationNavigation)
                    .WithMany(p => p.Principal)
                    .HasForeignKey(d => d.Organization)
                    .HasConstraintName("principal_organization_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project", "report");

                entity.HasIndex(e => e.Dossier)
                    .HasName("project_dossier_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("nextval('report.project_id_seq'::regclass)");
                entity.Property(e => e.Adviser).HasColumnName("adviser");
                entity.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP").ForNpgsqlHasComment("Timestamp of record creation, set by insert");
                entity.Property(e => e.Creator).HasColumnName("creator");
                entity.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of soft delete");
                entity.Property(e => e.Dossier).HasColumnName("dossier").HasMaxLength(256).ForNpgsqlHasComment("User provided dossier number, must be unique");
                entity.Property(e => e.EndDate).HasColumnName("end_date").HasColumnType("date");
                entity.Property(e => e.Lead).HasColumnName("lead");
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.Outline).HasColumnName("outline");
                entity.Property(e => e.StartDate).HasColumnName("start_date").HasColumnType("date");
                entity.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.HasOne(d => d.AdviserNavigation)
                    .WithMany(p => p.ProjectAdviserNavigation)
                    .HasForeignKey(d => d.Adviser)
                    .HasConstraintName("project_adviser_fkey");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.ProjectCreatorNavigation)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("project_creator_fkey");

                entity.HasOne(d => d.LeadNavigation)
                    .WithMany(p => p.ProjectLeadNavigation)
                    .HasForeignKey(d => d.Lead)
                    .HasConstraintName("project_lead_fkey");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.HasKey(e => new { e.Id, e.DocumentId });

                entity.ToTable("report", "report");

                entity.HasIndex(e => e.Id)
                    .HasName("report_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("nextval('report.report_id_seq'::regclass)");
                entity.Property(e => e.DocumentId).HasColumnName("document_id").HasMaxLength(64).ForNpgsqlHasComment("User provided document identifier");
                entity.Property(e => e.ConformF3o).HasColumnName("conform_f3o");
                entity.Property(e => e.Contractor).HasColumnName("contractor");
                entity.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP").ForNpgsqlHasComment("Timestamp of record creation, set by insert");
                entity.Property(e => e.Creator).HasColumnName("creator");
                entity.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of soft delete");
                entity.Property(e => e.DocumentDate).HasColumnName("document_date").HasColumnType("date");
                entity.Property(e => e.DocumentName).HasColumnName("document_name").HasMaxLength(128);
                entity.Property(e => e.FloorMeasurement).HasColumnName("floor_measurement");
                entity.Property(e => e.Inspection).HasColumnName("inspection");
                entity.Property(e => e.JointMeasurement).HasColumnName("joint_measurement");
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.Owner).HasColumnName("owner");
                entity.Property(e => e.Project).HasColumnName("project");
                entity.Property(e => e.Reviewer).HasColumnName("reviewer");
                entity.Property(e => e.AccessPolicy).HasColumnName("access_policy").HasConversion(new AccessPolicyConverter());
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(32).HasDefaultValueSql("'todo'::character varying");
                entity.Property(e => e.Type).IsRequired().HasColumnName("type").HasMaxLength(32).HasDefaultValueSql("'unknown'::character varying");
                entity.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.HasOne(d => d.ContractorNavigation)
                    .WithMany(p => p.ReportContractorNavigation)
                    .HasForeignKey(d => d.Contractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_contractor_fkey");

                entity.HasOne(d => d.CreatorNavigation)
                    .WithMany(p => p.ReportCreatorNavigation)
                    .HasForeignKey(d => d.Creator)
                    .HasConstraintName("report_creator_fkey");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.ReportOwnerNavigation)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_owner_fkey");

                entity.HasOne(d => d.ProjectNavigation)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Project)
                    .HasConstraintName("report_project_fkey");

                entity.HasOne(d => d.ReviewerNavigation)
                    .WithMany(p => p.ReportReviewerNavigation)
                    .HasForeignKey(d => d.Reviewer)
                    .HasConstraintName("report_reviewer_fkey");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("report_status_fkey");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("report_type_fkey");
            });

            modelBuilder.Entity<ReportStatus>(entity =>
            {
                entity.ToTable("report_status", "report");

                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);
            });

            modelBuilder.Entity<ReportType>(entity =>
            {
                entity.ToTable("report_type", "report");

                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(54);
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.HasKey(e => new { e.Id, e.Report });

                entity.ToTable("sample", "report");

                entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("nextval('report.sample_id_seq'::regclass)");
                entity.Property(e => e.Report).HasColumnName("report").ForNpgsqlHasComment("Link to the report entity");
                entity.Property(e => e.BuildingNumber).HasColumnName("building_number");
                entity.Property(e => e.BuildingNumberSuffix).HasColumnName("building_number_suffix").HasColumnType("character(2)");
                entity.Property(e => e.BuiltYear).HasColumnName("built_year").HasColumnType("report.year");
                entity.Property(e => e.Cpt).HasColumnName("CPT").HasMaxLength(32);
                entity.Property(e => e.CreateDate).HasColumnName("create_date").HasColumnType("timestamp with time zone").HasDefaultValueSql("CURRENT_TIMESTAMP").ForNpgsqlHasComment("Timestamp of record creation, set by insert");
                entity.Property(e => e.DeleteDate).HasColumnName("delete_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of soft delete");
                entity.Property(e => e.FoundationDamageCause).IsRequired().HasColumnName("foundation_damage_cause").HasMaxLength(32).HasDefaultValueSql("'unknown'::character varying");
                entity.Property(e => e.FoundationQuality).HasColumnName("foundation_quality").HasMaxLength(32);
                entity.Property(e => e.FoundationRecoveryAdviced).HasColumnName("foundation_recovery_adviced");
                entity.Property(e => e.FoundationType).HasColumnName("foundation_type").HasMaxLength(32);
                entity.Property(e => e.GroudLevel).HasColumnName("groudlevel").HasColumnType("report.height");
                entity.Property(e => e.GroundwaterLevel).HasColumnName("groundwater_level").HasColumnType("report.height");
                entity.Property(e => e.MonitoringWell).HasColumnName("monitoring_well").HasMaxLength(32);
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.StreetName).IsRequired().HasColumnName("street_name").HasMaxLength(128);
                entity.Property(e => e.AccessPolicy).HasColumnName("access_policy").HasConversion(new AccessPolicyConverter());
                entity.Property(e => e.Substructure).HasColumnName("substructure").HasMaxLength(32);
                entity.Property(e => e.UpdateDate).HasColumnName("update_date").HasColumnType("timestamp with time zone").ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");
                entity.Property(e => e.WoodLevel).HasColumnName("wood_level").HasColumnType("report.height");

                entity.HasOne(d => d.FoundationDamageCauseNavigation)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => d.FoundationDamageCause)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sample_foundation_damage_case_fkey");

                entity.HasOne(d => d.FoundationQualityNavigation)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => d.FoundationQuality)
                    .HasConstraintName("sample_foundation_quality_fkey");

                entity.HasOne(d => d.FoundationTypeNavigation)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => d.FoundationType)
                    .HasConstraintName("sample_foundation_type_fkey");

                entity.HasOne(d => d.ReportNavigation)
                    .WithMany(p => p.Sample)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.Report)
                    .HasConstraintName("sample_report_fkey");

                entity.HasOne(d => d.SubstructureNavigation)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => d.Substructure)
                    .HasConstraintName("sample_substructure_fkey");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => new { d.StreetName, d.BuildingNumber, d.BuildingNumberSuffix })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("sample_street_name_fkey");
            });

            modelBuilder.Entity<Substructure>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(32).ValueGeneratedNever();
                entity.Property(e => e.NameNl).IsRequired().HasColumnName("name_nl").HasMaxLength(64);

                entity.ToTable("substructure", "report");
            });

            modelBuilder.HasSequence<int>("organization_id_seq");

            modelBuilder.HasSequence<int>("principal_id_seq").IncrementsBy(5);

            modelBuilder.HasSequence<int>("project_id_seq");

            modelBuilder.HasSequence<int>("report_id_seq");

            modelBuilder.HasSequence<int>("sample_id_seq");
        }
    }
}
