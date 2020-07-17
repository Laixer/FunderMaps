using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Tests.Mocks;
using FunderMaps.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunderMaps.Core.Tests.UseCases
{
    public class GeocoderUseCaseTests
    {
        [Fact]
        public async void GetAsyncReturnsIncident()
        {
            // Arrange
            var addressRepository = new MockAddressRepository();
            var useCase = new GeocoderUseCase(addressRepository.Object);

            // Act
            //var incident = await useCase.GetAsync("FIR012020-12345");

            // Assert
            //Assert.NotNull(incident);
            //Assert.IsAssignableFrom<Incident>(incident);
        }
    }
}
