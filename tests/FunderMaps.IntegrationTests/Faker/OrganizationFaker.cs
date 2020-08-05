using Bogus;
using Bogus.Extensions;
using Bogus.Extensions.UnitedStates;
using FunderMaps.Core.Entities;
using System;

namespace FunderMaps.IntegrationTests.Faker
{
    public class OrganizationFaker : Faker<Organization>
    {
        public OrganizationProposal Proposal { get; } = new OrganizationProposalFaker().Generate();

        public OrganizationFaker()
        {
            RuleFor(f => f.Id, f => Proposal.Id);
            RuleFor(f => f.Name, f => Proposal.Name);
            RuleFor(f => f.Email, f => Proposal.Email);
            RuleFor(f => f.PhoneNumber, f => f.Phone.PhoneNumber("###########").OrNull(f, .3f));
            RuleFor(f => f.RegistrationNumber, f => f.Company.Ein().OrNull(f, .7f));
            RuleFor(f => f.BrandingLogo, f => f.Company.Ein()); //
            RuleFor(f => f.InvoiceName, f => Proposal.Name); //
            RuleFor(f => f.InvoicePoBox, f => f.Address.ZipCode()); //
            RuleFor(f => f.InvoiceEmail, (f, o) => f.Internet.Email(o.Name));
            RuleFor(f => f.HomeStreet, f => f.Address.StreetName());
            RuleFor(f => f.HomeAddressNumber, f => Convert.ToInt32(f.Address.BuildingNumber()));
            RuleFor(f => f.HomeAddressNumberPostfix, f => f.Address.SecondaryAddress());
            RuleFor(f => f.HomeCity, f => f.Address.City());
            RuleFor(f => f.HomePostbox, f => f.Address.ZipCode()); //
            RuleFor(f => f.HomeZipcode, f => f.Address.ZipCode());
            RuleFor(f => f.HomeState, f => f.Address.State());
            RuleFor(f => f.HomeCountry, f => f.Address.Country());
            RuleFor(f => f.PostalStreet, f => f.Address.StreetName());
            RuleFor(f => f.PostalAddressNumber, f => Convert.ToInt32(f.Address.BuildingNumber()));
            RuleFor(f => f.PostalAddressNumberPostfix, f => f.Address.SecondaryAddress());
            RuleFor(f => f.PostalCity, f => f.Address.City());
            RuleFor(f => f.PostalPostbox, f => f.Address.ZipCode()); //
            RuleFor(f => f.PostalZipcode, f => f.Address.ZipCode());
            RuleFor(f => f.PostalState, f => f.Address.State());
            RuleFor(f => f.PostalCountry, f => f.Address.Country());
        }
    }
}
