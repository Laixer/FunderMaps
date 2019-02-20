using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using FunderMaps.Models;
using FunderMaps.Helpers;

namespace FunderMaps.Data.Seed
{
    public class FunderMapsSeed
    {
        public static async Task SeedAsync(IHostingEnvironment env, FunderMapsDbContext catalogContext)
        {
            // Check if seeding is done already.
            if (catalogContext.OrganizationRoles.Any()) { return; }

            await catalogContext.OrganizationRoles.AddRangeAsync(
                new OrganizationRole { Name = Constants.SuperuserRole, NormalizedName = Constants.SuperuserRole.ToUpper() },
                new OrganizationRole { Name = Constants.VerifierRole, NormalizedName = Constants.VerifierRole.ToUpper() },
                new OrganizationRole { Name = Constants.WriterRole, NormalizedName = Constants.WriterRole.ToUpper() },
                new OrganizationRole { Name = Constants.ReaderRole, NormalizedName = Constants.ReaderRole.ToUpper() }
            );
            await catalogContext.SaveChangesAsync();

            if (env.IsDevelopment() || env.IsStaging())
            {
                await SeedTestingDataAsync(catalogContext);
            }
        }

        private static async Task SeedTestingDataAsync(FunderMapsDbContext catalogContext)
        {
            await catalogContext.OrganizationProposals.AddAsync(new OrganizationProposal("Blub Corp.", "info@blub.com")
            {
                NormalizedName = "BLUB CORP."
            });
            await catalogContext.SaveChangesAsync();

            var address = new Address
            {
                Street = "Mainstreet",
                AddressNumber = 123,
                AddressNumberPostfix = "b",
                City = "Phoenix",
                Postbox = "93789",
                Zipcode = "92641",
                State = "Colorado",
                Country = "USA"
            };
            await catalogContext.Addresses.AddAsync(address);

            var organization = new Organization
            {
                Name = "Consoto",
                NormalizedName = "CONTOSO",
                Email = "info@consoto.com",
                PhoneNumber = "1900362185",
                RegistrationNumber = "XGHA83",
                IsDefault = true,
                IsValidated = true,
                HomeAddress = address,
                PostalAddres = address,
            };
            await catalogContext.Organizations.AddAsync(organization);

            await catalogContext.SaveChangesAsync();
        }
    }
}
