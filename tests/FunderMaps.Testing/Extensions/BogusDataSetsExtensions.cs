using Bogus;
using Bogus.DataSets;

namespace FunderMaps.Testing.Extensions
{
    public static class BogusDataSetsExtensions
    {
        // FUTURE: Secure path may not be necessary anymore
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
            => randomizer.String2(length, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_-==+`~");
    }
}
