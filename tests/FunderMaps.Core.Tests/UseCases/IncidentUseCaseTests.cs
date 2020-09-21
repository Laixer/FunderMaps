using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
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
            // Arrange.
            var fileStorageService = new Mock<IBlobStorageService>();
            var contactRepository = new Mock<IContactRepository>();
            var incidentRepository = new MockIncidentRepository().MockGetByIdAsync(new Incident());
            var addressRepository = new MockAddressRepository().MockGetByIdAsync(new Address());
            var useCase = new IncidentUseCase(
                fileStorageService.Object,
                contactRepository.Object,
                incidentRepository.Object,
                addressRepository.Object);

            // Act.
            var incident = await useCase.GetAsync("FIR012020-12345");

            // Assert.
            Assert.NotNull(incident);
            Assert.IsType<Incident>(incident);
        }
    }
}
