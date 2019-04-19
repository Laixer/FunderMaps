using System;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecoveryRepair
    {
        public string Location { get; set; }
        public int Recovery { get; set; }

        public virtual FoundationRecoveryLocation LocationNavigation { get; set; }
        public virtual FoundationRecovery RecoveryNavigation { get; set; }
    }
}
