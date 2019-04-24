using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using FunderMaps.Core.Extensions;

namespace FunderMaps.Data.Converters
{
    /// <summary>
    /// Convert string to enum and back with snake case string conversion.
    /// </summary>
    public class EnumSnakeCaseConverter<TEnum> : ValueConverter<TEnum, string>
        where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumSnakeCaseConverter{TEnum}" /> class.
        /// </summary>
        public EnumSnakeCaseConverter(ConverterMappingHints mappingHints = null)
            : base(v => v.ToString().ToUnderscore(),
                  v => (TEnum)Enum.Parse(typeof(TEnum), v, true),
                  mappingHints)
        {
        }

        /// <summary>
        /// A <see cref="ValueConverterInfo" /> for the default use of this converter.
        /// </summary>
        public static ValueConverterInfo DefaultInfo { get; }
            = new ValueConverterInfo(typeof(TEnum), typeof(string), i => new EnumSnakeCaseConverter<TEnum>(i.MappingHints));
    }
}
