using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Moq;
using System.Threading.Tasks;

namespace FunderMaps.Core.Tests.Mocks
{
    internal class MockIncidentRepository : Mock<IIncidentRepository>
    {
        public MockIncidentRepository MockGetByIdAsync(Incident result)
        {
            Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<Incident>(result));

            return this;
        }

        public MockIncidentRepository MockGetByIdAsyncInvalid()
        {
            Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Throws(new EntityNotFoundException());

            return this;
        }
    }
}
