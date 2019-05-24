using System;

namespace FunderMaps.Core.Entities.Fis
{
    public class Object
    {
        public Guid Id { get; set; }
        public long? Bag { get; set; }

        public virtual Address IdNavigation { get; set; }
    }
}
