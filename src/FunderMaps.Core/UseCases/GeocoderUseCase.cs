using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Core.UseCases
{
    /// <summary>
    ///     Geocoder use case.
    /// </summary>
    public class GeocoderUseCase
    {
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public GeocoderUseCase(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        /// <summary>
        ///     Get address.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public virtual async ValueTask<Address> GetAsync(string id)
        {
            Validator.ValidateValue(id, new ValidationContext(id), new List<AddressAttribute> { new AddressAttribute() });

            return await _addressRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        // TODO: Add cache

        /// <summary>
        ///     Retrieve all addresses matching search query.
        /// </summary>
        /// <param name="query">Search query.</param>
        /// <param name="navigation">Recordset nagivation.</param>
        public IAsyncEnumerable<Address> GetAllBySuggestionAsync(string query, INavigation navigation)
            => _addressRepository.GetBySearchQueryAsync(query, navigation);
    }
}
