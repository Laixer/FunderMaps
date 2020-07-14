using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Tests.Mocks;
using FunderMaps.Core.UseCases;
using Moq;
using Xunit;

namespace FunderMaps.Core.Tests.UseCases
{
    public class IncidentUseCaseTests
    {
        [Fact]
        public async void GetAsyncReturnsIncident()
        {
            // Arrange
            var contactRepository = new Mock<IContactRepository>();
            var incidentRepository = new MockIncidentRepository().MockGetByIdAsync(new Incident());
            var useCase = new IncidentUseCase(contactRepository.Object, incidentRepository.Object);

            // Act
            var incident = await useCase.GetAsync(It.IsAny<string>());

            // Assert
            Assert.NotNull(incident);
            Assert.IsAssignableFrom<Incident>(incident);
        }
    }
}
