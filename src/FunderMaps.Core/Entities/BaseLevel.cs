using NpgsqlTypes;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Base measurement level.
    /// </summary>
    public enum BaseLevel
    {
        /// <summary>
        /// Normaal Amsterdams Peil.
        /// </summary>
        [PgName("nap")]
        [EnumMember(Value = "nap")]
        NAP,

        /// <summary>
        /// Tweede Algemene Waterpassing.
        /// </summary>
        [PgName("taw")]
        [EnumMember(Value = "taw")]
        TAW,

        /// <summary>
        /// Normalnull.
        /// </summary>
        [PgName("nn")]
        [EnumMember(Value = "nn")]
        NN,
    }
}
