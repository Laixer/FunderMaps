using System;

namespace FunderMaps.Models.Fis
{
    public class Norm
    {
        public int Id { get; set; }
        public bool? ConformF3o { get; set; }

        public virtual Report IdNavigation { get; set; }
    }
}
