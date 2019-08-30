﻿using System;
using System.Threading.Tasks;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Interfaces
{
    public interface IAddressRepository : IAsyncRepository<Address, Guid>
    {
        /// <summary>
        /// Get existing address entity from data store or insert given
        /// as new entity.
        /// </summary>
        /// <param name="address">Input address.</param>
        /// <returns>See <see cref="Address"/>.</returns>
        Task<Address> GetOrAddAsync(Address address);
    }
}
