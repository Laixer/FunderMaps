using FunderMaps.Core.Entities;
using FunderMaps.Core.Tests.Mocks;
using FunderMaps.Core.UseCases;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.Core.Tests.UseCases
{
    public class GeocoderUseCaseTests
    {
        [Fact]
        public async void GetAsyncReturnsAddress()
        {
            // Arrange
            var addressRepository = new MockAddressRepository().MockGetByIdAsync(new Address());
            var useCase = new GeocoderUseCase(addressRepository.Object);

            // Act
            var address = await useCase.GetAsync("gfm-1af01083838148deb80d62960f1e8f83");

            // Assert
            Assert.NotNull(address);
            Assert.IsType<Address>(address);
        }

        [Fact]
        public async void GetAllBySuggestionAsyncAddresses()
        {
            static async IAsyncEnumerable<Address> GetAddressAsync()
            {
                yield return new Address();
                yield return new Address();
                yield return new Address();

                await Task.CompletedTask;
            }

            // Arrange
            var navigation = new MockNavigation();
            var addressRepository = new MockAddressRepository().MockGetBySearchQueryAsync(GetAddressAsync());
            var useCase = new GeocoderUseCase(addressRepository.Object);

            // Act
            var asyncEnumerable = useCase.GetAllBySuggestionAsync("gfm-1af01083838148deb80d62960f1e8f83", navigation.Object);
            var asyncEnumerator = asyncEnumerable.GetAsyncEnumerator();

            // Assert
            Assert.NotNull(asyncEnumerable);
            Assert.NotNull(asyncEnumerator);
            Assert.True(await asyncEnumerator.MoveNextAsync());
            Assert.IsType<Address>(asyncEnumerator.Current);
            Assert.True(await asyncEnumerator.MoveNextAsync());
        }
    }
}
