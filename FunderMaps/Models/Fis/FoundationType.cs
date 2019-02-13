﻿using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public partial class FoundationType
    {
        public FoundationType()
        {
            Sample = new HashSet<Sample>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        public ICollection<Sample> Sample { get; set; }
    }
}
