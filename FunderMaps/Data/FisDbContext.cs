using Microsoft.EntityFrameworkCore;
using FunderMaps.Data.Builder;
using FunderMaps.Models.Fis;

namespace FunderMaps.Data
{
    public partial class FisDbContext : DbContext
    {
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<FoundationDamageCause> FoundationDamageCause { get; set; }
        public virtual DbSet<FoundationQuality> FoundationQuality { get; set; }
        public virtual DbSet<FoundationRecovery> FoundationRecovery { get; set; }
        public virtual DbSet<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }
        public virtual DbSet<FoundationRecoveryType> FoundationRecoveryType { get; set; }
        public virtual DbSet<FoundationType> FoundationType { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Principal> Principal { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportStatus> ReportStatus { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<Sample> Sample { get; set; }
        public virtual DbSet<Substructure> Substructure { get; set; }

        public FisDbContext(DbContextOptions<FisDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Fis.ModelCreating(modelBuilder);
        }
    }
}
