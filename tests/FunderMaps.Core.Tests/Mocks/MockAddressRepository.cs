using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Tests.Mocks
{
    internal class MockAddressRepository : Mock<IAddressRepository>
    {
        public MockAddressRepository MockGetByIdAsync(Address result)
        {
            Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<Address>(result));

            return this;
        }

        public MockAddressRepository MockGetBySearchQueryAsync(IAsyncEnumerable<Address> result)
        {
            Setup(s => s.GetBySearchQueryAsync(It.IsAny<string>(), It.IsAny<INavigation>()))
                .Returns(result);

            return this;
        }
    }
}
