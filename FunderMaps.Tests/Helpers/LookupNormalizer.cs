using System;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Tests.Helpers
{
    public class LookupNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.Normalize().ToLowerInvariant();
        }
    }
}
