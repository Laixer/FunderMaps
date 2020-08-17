using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.UseCases;
using FunderMaps.IndicentEndpoint.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IndicentEndpoint.Tests
{
    public class HttpPortalEndpointTests
    {
        [Fact]
        public async void CreateIncidentReturnOk()
        {
            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            contactRepositoryMock
                .Setup(s => s.AddAsync(It.IsAny<Contact>()))
                .Returns(new ValueTask<string>("info@example.org"));
            contactRepositoryMock
                .Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<Contact>(new Contact()));
            var incidentRepositoryMock = new Mock<IIncidentRepository>();
            incidentRepositoryMock
                .Setup(s => s.AddAsync(It.IsAny<Incident>()))
                .Returns(new ValueTask<string>("FIR123"));
            incidentRepositoryMock
                .Setup(s => s.GetByIdAsync(It.IsAny<string>()))
                .Returns(new ValueTask<Incident>(new Incident
                {
                    Email = "info@example.org"
                }));
            var addressRepositoryMock = new Mock<IAddressRepository>();

            var incidentUseCase = new IncidentUseCase(contactRepositoryMock.Object, incidentRepositoryMock.Object);
            var geocoderUseCase = new GeocoderUseCase(addressRepositoryMock.Object);

            var httpRequest = new DefaultHttpContext();
            var endpoint = new HttpPortalEndpoint(incidentUseCase, geocoderUseCase);
            var input = new IncidentInputViewModel
            {
                ClientId = 42,
                Address = "gfm-12324345677",
                FoundationType = FoundationType.NoPileSlit,
                ChainedBuilding = true,
                Owner = true,
                FoundationRecovery = false,
                FoundationDamageCause = FoundationDamageCause.OverchargeNegativeCling,
                DocumentName = new string[] { "https://example.org/somefile.ext" },
                Note = "This is a note",
                FoundationDamageCharacteristics = new FoundationDamageCharacteristics[]
                {
                    FoundationDamageCharacteristics.CrawlspaceFlooding,
                    FoundationDamageCharacteristics.Skewed
                },
                EnvironmentDamageCharacteristics = new EnvironmentDamageCharacteristics[]
                {
                    EnvironmentDamageCharacteristics.IncreasingTraffic,
                    EnvironmentDamageCharacteristics.SaggingCablesPipes,
                    EnvironmentDamageCharacteristics.VegetationNearby,
                },
                Name = "Henk Kaas",
                Email = "info@kaas.com",
                Phonenumber = "06482734212",
            };

            // Act
            var response = await endpoint.AddIncident(input, httpRequest.Request);

            // Assert
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
