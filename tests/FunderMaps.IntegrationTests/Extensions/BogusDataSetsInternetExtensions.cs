namespace FunderMaps.IntegrationTests.Extensions
{
    public static class BogusDataSetsInternetExtensions
    {
        public static string RemoteFileWithSecureUrl(this Bogus.DataSets.Internet internet, string[] providedFileExt = null)
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
    }
}
