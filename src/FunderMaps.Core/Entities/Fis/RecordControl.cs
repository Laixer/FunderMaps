using System;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Record timestamps.
    /// </summary>
    public abstract class RecordControl : BaseEntity 
    {
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
