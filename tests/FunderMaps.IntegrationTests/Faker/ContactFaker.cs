using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Entities;

namespace FunderMaps.IntegrationTests.Faker
{
    public class ContactFaker : Faker<Contact>
    {
        public ContactFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.Name = f.Person.FullName.OrNull(f, .3f);
                o.PhoneNumber = f.Phone.PhoneNumber("###########").OrNull(f, .3f);
            });
        }
    }
}
