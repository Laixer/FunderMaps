using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    public class IncidentInputViewModel
    {
        public Address Address { get; set; } // geolocation
        public FoundationType FoundationType { get; set; }
        public bool ChainedBuilding { get; set; }
        public bool Owner { get; set; }
        public bool FoundationRecovery { get; set; }
        public FoundationDamageCause FoundationDamageCause { get; set; }
        public string DocumentName { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; set; }
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
    }
}
