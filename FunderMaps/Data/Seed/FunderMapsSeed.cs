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

            await catalogContext.Organizations.AddAsync(new Organization
            {
                Name = "Contoso",
                NormalizedName = "CONTOSO",
                Email = "info@contoso.com",
                PhoneNumber = "+1900731753",
                RegistrationNumber = "US6793426",
                IsDefault = true,
                IsValidated = true,
                InvoiceName = "Contoso",
                InvoicePONumber = "837-OR",
                InvoiceEmail = "invoice@contoso.com",
                AttestationOrganizationId = 0,

                HomeStreet = "Mainstreet",
                HomeAddressNumber = 123,
                HomeAddressNumberPostfix = "b",
                HomeCity = "Phoenix",
                HomePostbox = "93789",
                HomeZipcode = "92641",
                HomeState = "Colorado",
                HomeCountry = "USA",

                PostalStreet = "Mainstreet",
                PostalAddressNumber = 123,
                PostalAddressNumberPostfix = "b",
                PostalCity = "Phoenix",
                PostalPostbox = "93789",
                PostalZipcode = "92641",
                PostalState = "Colorado",
                PostalCountry = "USA",
            });
            await catalogContext.SaveChangesAsync();
        }
    }
}
