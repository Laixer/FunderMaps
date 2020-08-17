using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.IndicentEndpoint.ViewModels
{
    /// <summary>
    ///     Incident input view model.
    /// </summary>
    public class IncidentInputViewModel
    {
        /// <summary>
        ///     Client identifier.
        /// </summary>
        [Required, Range(1, 99)]
        public int ClientId { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        [Required, Geocoder]
        public string Address { get; set; }

        /// <summary>
        ///     Foundation type.
        /// </summary>
        [Required]
        public FoundationType FoundationType { get; set; }

        /// <summary>
        ///     Building chained to another building.
        /// </summary>
        [Required]
        public bool ChainedBuilding { get; set; }

        /// <summary>
        ///     Whether the contact is an owner of the building.
        /// </summary>
        [Required]
        public bool Owner { get; set; }

        /// <summary>
        ///     Whether foundation was recovered or not.
        /// </summary>
        [Required]
        public bool FoundationRecovery { get; set; }

        /// <summary>
        ///     Foundation damage cause.
        /// </summary>
        [Required]
        public FoundationDamageCause FoundationDamageCause { get; set; }

        /// <summary>
        ///     Document name.
        /// </summary>
        public string[] DocumentName { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///     Foundational damage.
        /// </summary>
        public FoundationDamageCharacteristics[] FoundationDamageCharacteristics { get; set; } = new FoundationDamageCharacteristics[0];

        /// <summary>
        ///     Environmental damage.
        /// </summary>
        public EnvironmentDamageCharacteristics[] EnvironmentDamageCharacteristics { get; set; } = new EnvironmentDamageCharacteristics[0];

        /// <summary>
        ///     Contact name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Contact email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Contact phone number.
        /// </summary>
        public string Phonenumber { get; set; }
    }
}
