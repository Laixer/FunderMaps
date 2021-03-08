using Bogus;
using Bogus.DataSets;

namespace FunderMaps.Testing.Extensions
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

            if (providedFileExt is not null)
            {
                fileExt = providedFileExt;
            }

            return internet.UrlWithPath("https", internet.DomainName(), internet.Random.ArrayElement(fileExt));
        }

        public static string Password(this Randomizer randomizer, int length = 12)
        {
            return randomizer.String2(length, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
        }
    }
}
