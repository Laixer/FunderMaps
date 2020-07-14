using FunderMaps.Core.Entities;
using FunderMaps.IndicentEndpoint.ViewModels;
using System;

namespace FunderMaps.IndicentEndpoint.Extensions
{
    /// <summary>
    ///     Extend incident.
    /// </summary>
    internal static class IncidentExtensions
    {
        /// <summary>
        ///     Map input viewmodel to incident entity.
        /// </summary>
        /// <param name="incident">Instance to extend.</param>
        /// <param name="input">Input from <see cref="IncidentInputViewModel"/>.</param>
        public static Incident MapFrom(this Incident incident, IncidentInputViewModel input)
        {
            if (incident == null)
            {
                throw new ArgumentNullException(nameof(incident));
            }

            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            incident.Address = input.Address;
            incident.FoundationType = input.FoundationType;
            incident.ChainedBuilding = input.ChainedBuilding;
            incident.Owner = input.Owner;
            incident.FoundationRecovery = input.FoundationRecovery;
            incident.FoundationDamageCause = input.FoundationDamageCause;
            incident.DocumentFile = input.DocumentName;
            incident.Note = input.Note;
            incident.FoundationDamageCharacteristics = input.FoundationDamageCharacteristics;
            incident.EnvironmentDamageCharacteristics = input.EnvironmentDamageCharacteristics;
            incident.Email = input.Email;
            incident.ContactNavigation = new Contact
            {
                Name = input.Name,
                Email = input.Email,
                PhoneNumber = input.Phonenumber
            };
            return incident;
        }
    }
}
