using System;
using FunderMaps.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Data
{
    public class FunderMapsIdentityDbContext : IdentityDbContext<FunderMapsUser, FunderMapsRole, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public FunderMapsIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected FunderMapsIdentityDbContext()
        {
        }

        // NOTE: We're not calling the base method since that would override the
        // entity properties.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Builder.Identity.ModelCreating(modelBuilder);
        }
    }
}
