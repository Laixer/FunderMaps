#if KAASS
namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Foundation recovery repair.
    /// </summary>
    public class FoundationRecoveryRepair
    {
        /// <summary>
        /// Repair location.
        /// </summary>
        public FoundationRecoveryLocation Location { get; set; }

        /// <summary>
        /// Recovery identifier.
        /// </summary>
        public int Recovery { get; set; }

        /// <summary>
        /// Foundation recovery object.
        /// </summary>
        public Recovery RecoveryNavigation { get; set; }
    }
}
#endif