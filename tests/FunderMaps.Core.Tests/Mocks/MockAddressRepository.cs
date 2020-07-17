using FunderMaps.Core.Interfaces.Repositories;
using Moq;

namespace FunderMaps.Core.Tests.Mocks
{
    internal class MockAddressRepository : Mock<IAddressRepository>
    {
        //public MockIncidentRepository MockGetByIdAsync(Incident result)
        //{
        //    Setup(s => s.GetByIdAsync(It.IsAny<string>()))
        //        .Returns(new ValueTask<Incident>(result));

        //    return this;
        //}
    }
}
