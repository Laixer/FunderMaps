using Bogus;
using Bogus.Extensions;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.IntegrationTests.Faker
{
    public class UserFaker : Faker<User>
    {
        private readonly IEnumerable<string> jobs = new List<string>
        {
            "accountant",
            "actor",
            "air steward",
            "animator",
            "architect",
            "assistant",
            "author",
            "baker",
            "biologist",
            "builder",
            "butcher",
            "career counselor",
            "caretaker",
            "chef",
            "civil servant",
            "clerk",
            "comic book writer",
            "company director",
            "programmer",
            "cook",
            "decorator",
            "dentist",
            "designer",
            "diplomat",
            "director",
            "doctor",
            "economist",
            "editor",
            "electrician",
            "engineer",
            "executive",
            "farmer",
            "film director",
            "fisherman",
            "fishmonger",
            "flight attendant",
            "garbage man",
            "geologist",
            "hairdresser",
            "head teacher",
            "jeweler",
            "journalist",
            "judge",
            "juggler",
            "lawyer",
            "lecturer",
            "lexicographer",
            "library assistant",
            "magician",
            "makeup artist",
            "manager",
            "miner",
            "musician",
            "nurse",
            "optician",
            "painter",
            "personal assistant",
            "photographer",
            "pilot",
            "plumber",
            "police officer",
            "politician",
            "porter",
            "printer",
            "prison officer / warder",
            "professional gambler",
            "puppeteer",
            "receptionist",
            "sailor",
            "salesperson",
            "scientist",
            "secretary",
            "shop assistant",
            "sign language Interpreter",
            "singer",
            "soldier",
            "solicitor",
            "surgeon",
            "tailor",
            "teacher",
            "telephone operator",
            "telephonist",
            "translator",
            "travel agent",
            "trucker",
            "TV cameraman",
            "TV presenter",
            "vet",
            "waiter",
            "web designer",
            "writer",
        };

        public UserFaker()
        {
            RuleFor(f => f.Id, f => f.Random.Uuid());
            RuleFor(f => f.GivenName, f => f.Person.FirstName.OrNull(f, .1f));
            RuleFor(f => f.LastName, f => f.Person.LastName.OrNull(f, .1f));
            RuleFor(f => f.Email, f => f.Person.Email);
            RuleFor(f => f.Avatar, f => f.Person.Avatar.OrNull(f, .8f));
            RuleFor(f => f.JobTitle, f => f.Random.ArrayElement(jobs.ToArray()));
            RuleFor(f => f.PhoneNumber, f => f.Phone.PhoneNumber("###########").OrNull(f, .3f));
            RuleFor(f => f.Role, f => f.PickRandom<ApplicationRole>());
        }
    }
}
