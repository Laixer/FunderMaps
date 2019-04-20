using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FunderMaps.Core.Entities.Fis;

namespace FunderMaps.Data.Converters
{
    /// <summary>
    /// Convert string to Access Policy enum and back.
    /// </summary>
    public class AccessPolicyConverter : ValueConverter<AccessPolicy, string>
    {
        public AccessPolicyConverter()
            : base(v => v.ToString().ToLower(),
                  v => (AccessPolicy)Enum.Parse(typeof(AccessPolicy), v, true))
        {
        }
    }
}
