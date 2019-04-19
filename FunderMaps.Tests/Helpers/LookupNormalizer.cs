using System;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Tests.Helpers
{
    internal class LookupNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.Normalize().ToLowerInvariant();
        }
    }
}
