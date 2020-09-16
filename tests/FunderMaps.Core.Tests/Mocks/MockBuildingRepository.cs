using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Moq;
using System.Threading.Tasks;

namespace FunderMaps.Core.Tests.Mocks
{
    internal class MockBuildingRepository : Mock<IBuildingRepository>
    {
        public MockBuildingRepository MockGetByIdAsync(Building result)
        {
            Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<Building>(result));

            return this;
        }
    }
}
