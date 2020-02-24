using FunderMaps.Core.Entities;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class IncidentInputViewModel
    {
        public Address Address { get; set; }
        public FoundationType FoundationType { get; set; }
        public bool ChainedBuilding { get; set; }
        public string DocumentName { get; set; }
        public bool Owner { get; set; }
        public bool FoundationRecovery { get; set; }
        public FoundationDamageCause FoundationDamageCause { get; set; }
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; set; }
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
    }
}
