using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Translate any geocoder identifier to an internal entity.
    /// </summary>
    public class GeocoderTranslation : IGeocoderTranslation
    {
        private readonly IGeocoderParser _geocoderParser;
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public GeocoderTranslation(IGeocoderParser geocoderParser, IAddressRepository addressRepository)
        {
            _geocoderParser = geocoderParser ?? throw new ArgumentNullException(nameof(geocoderParser));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        /// <summary>
        ///     Convert geocoder identifier to address entity.
        /// </summary>
        /// <param name="input">Input identifier.</param>
        /// <returns>If found returns the <see cref="Address"/> entity.</returns>
        public async Task<Address> GetAddressIdAsync(string input)
            => _geocoderParser.FromIdentifier(input, out string id) switch
            {
                GeocoderDatasource.FunderMaps => await _addressRepository.GetByIdAsync(id),
                GeocoderDatasource.NlBagAddress => await _addressRepository.GetByExternalIdAsync(id, ExternalDataSource.NlBag),
                _ => throw new EntityNotFoundException("Requested address entity could not be found."),
            };
    }
}
