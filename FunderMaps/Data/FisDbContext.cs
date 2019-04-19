using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data.Builder;
using FunderMaps.Models.Fis;
using FunderMaps.Models;

namespace FunderMaps.Data
{
    public class FisDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public FisDbContext(DbContextOptions<FisDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected FisDbContext()
        {
        }

        public virtual DbSet<AccessPolicy> AccessPolicy { get; set; }
        public virtual DbSet<Models.Fis.Address> Address { get; set; }
        public virtual DbSet<Attribution> Attribution { get; set; }
        public virtual DbSet<BaseLevel> BaseLevel { get; set; }
        public virtual DbSet<EnforcementTerm> EnforcementTerm { get; set; }
        public virtual DbSet<FoundationDamageCause> FoundationDamageCause { get; set; }
        public virtual DbSet<FoundationQuality> FoundationQuality { get; set; }
        public virtual DbSet<FoundationRecovery> FoundationRecovery { get; set; }
        public virtual DbSet<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }
        public virtual DbSet<FoundationRecoveryEvidenceType> FoundationRecoveryEvidenceType { get; set; }
        public virtual DbSet<FoundationRecoveryLocation> FoundationRecoveryLocation { get; set; }
        public virtual DbSet<FoundationRecoveryRepair> FoundationRecoveryRepair { get; set; }
        public virtual DbSet<FoundationRecoveryType> FoundationRecoveryType { get; set; }
        public virtual DbSet<FoundationType> FoundationType { get; set; }
        public virtual DbSet<Incident> Incident { get; set; }
        public virtual DbSet<Norm> Norm { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<Models.Fis.Organization> Organization { get; set; }
        public virtual DbSet<Principal> Principal { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportStatus> ReportStatus { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<Sample> Sample { get; set; }
        public virtual DbSet<Substructure> Substructure { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Fis.ModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChecEntityDelete();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChecEntityDelete();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Always initiate a soft delete if entity is configured so, otherwise ignore deletes
        /// since the data store credentials never allow a hard delete.
        /// </summary>
        private void ChecEntityDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ISoftDeletable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            entry.Property("DeleteDate").CurrentValue = System.DateTime.Now;
                            entry.State = EntityState.Modified;
                            break;
                        case EntityState.Added:
                            entry.Property("DeleteDate").CurrentValue = null;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Unchanged;
                    }
                }
            }
        }
    }
}
