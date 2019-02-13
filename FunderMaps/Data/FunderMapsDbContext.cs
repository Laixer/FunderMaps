using Microsoft.EntityFrameworkCore;
using FunderMaps.Models;

namespace FunderMaps.Data
{
    public class FunderMapsDbContext : FunderMapsIdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public FunderMapsDbContext(DbContextOptions<FunderMapsDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected FunderMapsDbContext()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Address.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Organizations.
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of OrganizationProposals.
        /// </summary>
        public DbSet<OrganizationProposal> OrganizationProposals { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of OrganizationUsers.
        /// </summary>
        public DbSet<OrganizationUser> OrganizationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Builder.FunderMaps.ModelCreating(modelBuilder);
        }
    }
}
