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
        [EnumMember(Value = "nap")]
        NAP,

        /// <summary>
        /// Tweede Algemene Waterpassing.
        /// </summary>
        [EnumMember(Value = "taw")]
        TAW,

        /// <summary>
        /// Normalnull.
        /// </summary>
        [EnumMember(Value = "nn")]
        NN,
    }
}
