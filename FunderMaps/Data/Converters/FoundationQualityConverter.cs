using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Extensions;

namespace FunderMaps.Data.Converters
{
    /// <summary>
    /// Convert string to Access Policy enum and back.
    /// </summary>
    public class FoundationQualityConverter : ValueConverter<FoundationQuality, string>
    {
        public FoundationQualityConverter()
            : base(v => v.ToString().ToUnderscore(),
                  v => (FoundationQuality)Enum.Parse(typeof(FoundationQuality), v, true))
        {
        }
    }
}
