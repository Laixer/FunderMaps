namespace FunderMaps.Core.Entities
{
    public class FoundationRecoveryRepair
    {
        public FoundationRecoveryLocation Location { get; set; }
        public int Recovery { get; set; }

        public virtual FoundationRecovery RecoveryNavigation { get; set; }
    }
}
