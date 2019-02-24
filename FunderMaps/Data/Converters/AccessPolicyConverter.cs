using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FunderMaps.Models.Fis;

namespace FunderMaps.Data.Converters
{
    public class AccessPolicyConverter : ValueConverter<AccessPolicy, string>
    {
        public AccessPolicyConverter()
            : base(v => v.ToString().ToLower(),
                  v => (AccessPolicy)Enum.Parse(typeof(AccessPolicy), v, true))
        {
        }
    }
}
