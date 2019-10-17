using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Enforcement term.
    /// </summary>
    public enum EnforcementTerm
    {
        /// <summary>
        /// Between 0 - 5 years.
        /// </summary>
        [EnumMember(Value = "term_0-5")]
        Term0_5,

        /// <summary>
        /// Between 5 - 10 years.
        /// </summary>
        [EnumMember(Value = "term_5-10")]
        Term5_10,

        /// <summary>
        /// Between 10 - 20 years.
        /// </summary>
        [EnumMember(Value = "term_10-20")]
        Term10_20,

        /// <summary>
        /// 5 years.
        /// </summary>
        [EnumMember(Value = "term_5")]
        Term5,

        /// <summary>
        /// 10 years.
        /// </summary>
        [EnumMember(Value = "term_10")]
        Term10,

        /// <summary>
        /// 15 years.
        /// </summary>
        [EnumMember(Value = "term_15")]
        Term15,

        /// <summary>
        /// 20 years.
        /// </summary>
        [EnumMember(Value = "term_20")]
        Term20,

        /// <summary>
        /// 25 years.
        /// </summary>
        [EnumMember(Value = "term_25")]
        Term25,

        /// <summary>
        /// 30 years.
        /// </summary>
        [EnumMember(Value = "term_30")]
        Term30,
    }
}
