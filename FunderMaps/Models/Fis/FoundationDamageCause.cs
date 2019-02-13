﻿using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public partial class FoundationDamageCause
    {
        public FoundationDamageCause()
        {
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public ICollection<Sample> Sample { get; set; }
    }
}
