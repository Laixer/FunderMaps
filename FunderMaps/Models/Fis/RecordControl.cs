using System;

namespace FunderMaps.Models.Fis
{
    public abstract class RecordControl
    {
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
