using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class BaseLevel
    {
        public BaseLevel()
        {
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public virtual ICollection<Sample> Sample { get; set; }
    }
}
