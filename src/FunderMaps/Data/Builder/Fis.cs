using Microsoft.EntityFrameworkCore;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Data.Converters;

namespace FunderMaps.Data.Builder
{
    public static class Fis
    {
        public static void ModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address", "report");

                entity.HasIndex(e => new { e.StreetName, e.BuildingNumber, e.BuildingNumberSuffix })
                    .HasName("address_street_name_building_number_building_number_suffix_key")
                    .IsUnique();

                entity.HasIndex(e => new { e.StreetName, e.BuildingNumber })
                    .HasName("address_street_name_building_number_idx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.BuildingNumber).IsRequired().HasColumnName("building_number");

                entity.Property(e => e.BuildingNumberSuffix)
                    .HasColumnName("building_number_suffix")
                    .HasMaxLength(8);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Attribution>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("attribution_pkey");

                entity.ToTable("attribution", "report");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('report.attribution_id_seq'::regclass)");

                entity.Property(e => e._Contractor).HasColumnName("contractor");

                entity.Property(e => e._Creator).HasColumnName("creator");

                entity.Property(e => e._Owner).HasColumnName("owner");

                entity.Property(e => e.Project).HasColumnName("project");

                entity.Property(e => e._Reviewer).HasColumnName("reviewer");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.AttributionReviewerNavigation)
                    .HasForeignKey(d => d._Reviewer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attribution_reviewer_fkey");

                entity.HasOne(d => d.Contractor)
                    .WithMany(p => p.AttributionContractorNavigation)
                    .HasForeignKey(d => d._Contractor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attribution_contractor_fkey");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.AttributionCreatorNavigation)
                    .HasForeignKey(d => d._Creator)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attribution_creator_fkey");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.AttributionOwnerNavigation)
                    .HasForeignKey(d => d._Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attribution_owner_fkey");

                entity.HasOne(d => d.ProjectNavigation)
                    .WithMany(p => p.Attribution)
                    .HasForeignKey(d => d.Project)
                    .HasConstraintName("attribution_project_fkey");
            });

            modelBuilder.Entity<FoundationRecovery>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.ToTable("foundation_recovery", "report");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('report.foundation_recovery_id_seq'::regclass)");

                entity.Property(e => e.AccessPolicy)
                    .IsRequired()
                    .HasColumnName("access_policy")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'private'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<AccessPolicy>());

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Attribution).HasColumnName("attribution");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ForNpgsqlHasComment("Timestamp of record creation, set by insert");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of soft delete");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'unknown'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<FoundationRecoveryType>());

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.AddressNavigation)
                    .WithMany(p => p.FoundationRecovery)
                    .HasForeignKey(d => d.Address)
                    .HasConstraintName("foundation_recovery_address_fkey");

                entity.HasOne(d => d.AttributionNavigation)
                    .WithMany(p => p.FoundationRecovery)
                    .HasForeignKey(d => d.Attribution)
                    .HasConstraintName("foundation_recovery_attribution_fkey");
            });

            modelBuilder.Entity<FoundationRecoveryEvidence>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.HasKey(e => e.Document)
                    .HasName("foundation_recovery_evidence_pkey");

                entity.ToTable("foundation_recovery_evidence", "report");

                entity.Property(e => e.Document)
                    .HasColumnName("document")
                    .HasMaxLength(256)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ForNpgsqlHasComment("Timestamp of record creation, set by insert");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of soft delete");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(96);

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Recovery).HasColumnName("recovery");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationRecoveryEvidenceType>());

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.HasOne(d => d.RecoveryNavigation)
                    .WithMany(p => p.FoundationRecoveryEvidence)
                    .HasForeignKey(d => d.Recovery)
                    .HasConstraintName("foundation_recovery_evidence_recovery_fkey");
            });

            modelBuilder.Entity<FoundationRecoveryRepair>(entity =>
            {
                entity.HasKey(e => new { e.Location, e.Recovery })
                    .HasName("foundation_recovery_repair_pkey");

                entity.ToTable("foundation_recovery_repair", "report");

                entity.Property(e => e.Location)
                    .HasColumnName("location")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationRecoveryLocation>());

                entity.Property(e => e.Recovery).HasColumnName("recovery");

                entity.HasOne(d => d.RecoveryNavigation)
                    .WithMany(p => p.FoundationRecoveryRepair)
                    .HasForeignKey(d => d.Recovery)
                    .HasConstraintName("foundation_recovery_repair_recovery_fkey");
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("incident", "statement");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('statement.incident_id_seq'::regclass)");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.DocumentName)
                    .HasColumnName("document_name")
                    .HasMaxLength(256);

                entity.Property(e => e.FoundationDamageCause)
                    .HasColumnName("foundation_damage_cause")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'unknown'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<FoundationDamageCause>());

                entity.Property(e => e.FoundationQuality)
                    .HasColumnName("foundation_quality")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationQuality>());

                entity.Property(e => e.FoundationType)
                    .HasColumnName("foundation_type")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationType>());

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.Property(e => e.Substructure)
                    .HasColumnName("substructure")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<Substructure>());

                entity.HasOne(d => d.AddressNavigation)
                    .WithMany(p => p.Incident)
                    .HasForeignKey(d => d.Address)
                    .HasConstraintName("incident_address_fkey");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Incident)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("incident_owner_fkey");
            });

            modelBuilder.Entity<Norm>(entity =>
            {
                entity.ToTable("norm", "report");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(32)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConformF3o).HasColumnName("conform_f3o");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Norm)
                    .HasPrincipalKey<Report>(p => p.Id)
                    .HasForeignKey<Norm>(d => d.Id)
                    .HasConstraintName("norm_id_fkey");
            });

            modelBuilder.Entity<Object>(entity =>
            {
                entity.ToTable("object", "report");

                entity.HasIndex(e => e.Bag)
                    .HasName("object_bag_idx")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bag).HasColumnName("bag");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Object)
                    .HasForeignKey<Object>(d => d.Id)
                    .HasConstraintName("object_id_fkey");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("organization", "attestation");

                entity.HasIndex(e => e.Name)
                    .HasName("organization_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('attestation.organization_id_seq'::regclass)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
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

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('attestation.principal_id_seq'::regclass)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(256);

                entity.Property(e => e.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(256);

                entity.Property(e => e.MiddleName)
                    .HasColumnName("middle_name")
                    .HasMaxLength(256);

                entity.Property(e => e.NickName)
                    .HasColumnName("nick_name")
                    .HasMaxLength(256);

                entity.Property(e => e._Organization).HasColumnName("organization");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(16);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Principal)
                    .HasForeignKey(d => d._Organization)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("principal_organization_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.ToTable("project", "report");

                entity.HasIndex(e => e.Dossier)
                    .HasName("project_dossier_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('report.project_id_seq'::regclass)");

                entity.Property(e => e.Adviser).HasColumnName("adviser");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ForNpgsqlHasComment("Timestamp of record creation, set by insert");

                entity.Property(e => e.Creator).HasColumnName("creator");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of soft delete");

                entity.Property(e => e.Dossier)
                    .HasColumnName("dossier")
                    .HasMaxLength(256)
                    .ForNpgsqlHasComment("User provided dossier number, must be unique");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.Lead).HasColumnName("lead");

                entity.Property(e => e.Note).HasColumnName("note");

                //entity.Property(e => e.Outline).HasColumnName("outline");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

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

                entity.HasKey(e => new { e.Id, e.DocumentId })
                    .HasName("report_pkey");

                entity.ToTable("report", "report");

                entity.HasIndex(e => e.Id)
                    .HasName("report_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('report.report_id_seq'::regclass)");

                entity.Property(e => e.DocumentId)
                    .HasColumnName("document_id")
                    .HasMaxLength(64)
                    .ForNpgsqlHasComment("User provided document identifier");

                entity.Property(e => e.AccessPolicy)
                    .IsRequired()
                    .HasColumnName("access_policy")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'private'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<AccessPolicy>());

                entity.Property(e => e._Attribution).HasColumnName("attribution");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ForNpgsqlHasComment("Timestamp of record creation, set by insert");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of soft delete");

                entity.Property(e => e.DocumentDate)
                    .HasColumnName("document_date")
                    .HasColumnType("date");

                entity.Property(e => e.DocumentName)
                    .HasColumnName("document_name")
                    .HasMaxLength(256);

                entity.Property(e => e.FloorMeasurement).HasColumnName("floor_measurement");

                entity.Property(e => e.Inspection).HasColumnName("inspection");

                entity.Property(e => e.JointMeasurement).HasColumnName("joint_measurement");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'todo'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<ReportStatus>());

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'unknown'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<ReportType>());

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.HasOne(d => d.Attribution)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d._Attribution)
                    .HasConstraintName("report_attribution_fkey");
            });

            modelBuilder.Entity<Sample>(entity =>
            {
                entity.HasQueryFilter(e => e.DeleteDate == null);

                entity.HasKey(e => new { e.Id, e.Report })
                    .HasName("sample_pkey");

                entity.ToTable("sample", "report");

                entity.HasIndex(e => e.Id)
                    .HasName("sample_id_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('report.sample_id_seq'::regclass)");

                entity.Property(e => e.Report)
                    .HasColumnName("report")
                    .ForNpgsqlHasComment("Link to the report entity");

                entity.Property(e => e.AccessPolicy)
                    .IsRequired()
                    .HasColumnName("access_policy")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'private'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<AccessPolicy>());

                entity.Property(e => e._Address).HasColumnName("address");

                entity.Property(e => e.BaseMeasurementLevel)
                    .IsRequired()
                    .HasColumnName("base_measurement_level")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<BaseLevel>());

                entity.Property(e => e.BuiltYear).HasColumnName("built_year");

                entity.Property(e => e.Cpt)
                    .HasColumnName("cpt")
                    .HasMaxLength(32);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ForNpgsqlHasComment("Timestamp of record creation, set by insert");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("delete_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of soft delete");

                entity.Property(e => e.EnforcementTerm)
                    .HasColumnName("enforcement_term")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<EnforcementTerm>());

                entity.Property(e => e.FoundationDamageCause)
                    .IsRequired()
                    .HasColumnName("foundation_damage_cause")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("'unknown'::character varying")
                    .HasConversion(new EnumSnakeCaseConverter<FoundationDamageCause>());

                entity.Property(e => e.FoundationQuality)
                    .HasColumnName("foundation_quality")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationQuality>());

                entity.Property(e => e.FoundationRecoveryAdviced).HasColumnName("foundation_recovery_adviced");

                entity.Property(e => e.FoundationType)
                    .HasColumnName("foundation_type")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<FoundationType>());

                entity.Property(e => e.GroundLevel)
                    .HasColumnName("groundlevel")
                    .HasColumnType("numeric(5,2)");

                entity.Property(e => e.GroundwaterLevel)
                    .HasColumnName("groundwater_level")
                    .HasColumnType("numeric(5,2)");

                entity.Property(e => e.MonitoringWell)
                    .HasColumnName("monitoring_well")
                    .HasMaxLength(32);

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.Substructure)
                    .HasColumnName("substructure")
                    .HasMaxLength(32)
                    .HasConversion(new EnumSnakeCaseConverter<Substructure>());

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("timestamp with time zone")
                    .ForNpgsqlHasComment("Timestamp of last record update, automatically updated on record modification");

                entity.Property(e => e.WoodLevel)
                    .HasColumnName("wood_level")
                    .HasColumnType("numeric(5,2)");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Sample)
                    .HasForeignKey(d => d._Address)
                    .HasConstraintName("sample_address_fkey");

                entity.HasOne(d => d.ReportNavigation)
                    .WithMany(p => p.Sample)
                    .HasPrincipalKey(p => p.Id)
                    .HasForeignKey(d => d.Report)
                    .HasConstraintName("sample_report_fkey");
            });

            modelBuilder.HasSequence<int>("organization_id_seq");

            modelBuilder.HasSequence<int>("principal_id_seq").IncrementsBy(5);

            modelBuilder.HasSequence<int>("foundation_recovery_id_seq");

            modelBuilder.HasSequence<int>("project_id_seq");

            modelBuilder.HasSequence<int>("report_id_seq");

            modelBuilder.HasSequence<int>("sample_id_seq");
        }
    }
}
