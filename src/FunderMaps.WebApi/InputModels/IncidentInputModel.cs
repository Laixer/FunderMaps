using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.InputModels
{
    // TODO We need consistency for DTO / Viewmodel / Inputmodel (etc) naming
    /// <summary>
    ///     Incident input view model.
    /// </summary>
    public class IncidentInputModel
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
        public FoundationTypeDTO FoundationType { get; set; }

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
        public FoundationDamageCauseDTO FoundationDamageCause { get; set; }

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
        public FoundationDamageCharacteristicsDTO[] FoundationDamageCharacteristics { get; set; } = new FoundationDamageCharacteristicsDTO[0];

        /// <summary>
        ///     Environmental damage.
        /// </summary>
        public FoundationDamageCharacteristicsDTO[] EnvironmentDamageCharacteristics { get; set; } = new FoundationDamageCharacteristicsDTO[0];

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
