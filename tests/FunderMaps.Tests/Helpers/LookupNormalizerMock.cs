using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Tests.Helpers
{
    internal class LookupNormalizerMock : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.Normalize().ToLowerInvariant();
        }

        public string NormalizeEmail(string email)
        {
            return email.Normalize().ToLowerInvariant();
        }

        public string NormalizeName(string name)
        {
            return name.Normalize().ToLowerInvariant();
        }
    }
}
