using Bogus;
using Bogus.DataSets;
using System.Collections.Generic;

namespace FunderMaps.IntegrationTests.Extensions
{
    public static class BogusDataSetsExtensions
    {
        public static string RemoteFileWithSecureUrl(this Internet internet, string[] providedFileExt = null)
        {
            var fileExt = new string[]
            {
                ".pdf",
                ".png",
                ".gif",
                ".txt",
            };

            if (providedFileExt != null)
            {
                fileExt = providedFileExt;
            }

            return internet.UrlWithPath("https", internet.DomainName(), internet.Random.ArrayElement(fileExt));
        }

        public static List<T> Generate<T>(this Faker<T> faker, int min = int.MinValue, int max = int.MaxValue)
             where T : class
        {
            return faker.Generate(new Randomizer().Int(min, max));
        }

        public static string Password(this Randomizer randomizer, int length = 12)
        {
            return randomizer.String2(length, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
        }
    }
}
