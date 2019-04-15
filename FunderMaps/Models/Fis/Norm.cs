﻿using System;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Norm
    {
        [IgnoreDataMember]
        public int Id { get; set; }

        public bool? ConformF3o { get; set; }

        [IgnoreDataMember]
        public virtual Report IdNavigation { get; set; }
    }
}
