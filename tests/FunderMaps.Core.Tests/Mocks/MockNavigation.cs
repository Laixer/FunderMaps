using FunderMaps.Core.Interfaces;
using Moq;

namespace FunderMaps.Core.Tests.Mocks
{
    internal class MockNavigation : Mock<INavigation>
    {
        public MockNavigation()
        {
            Setup(s => s.Offset).Returns(0);
            Setup(s => s.Limit).Returns(100);
        }
    }
}
