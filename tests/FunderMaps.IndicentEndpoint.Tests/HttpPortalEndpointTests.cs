using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.UseCases;
using FunderMaps.IndicentEndpoint.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

// FUTURE: Rename to IncidentEndpoint
namespace FunderMaps.IndicentEndpoint.Tests
{
    // FUTURE: We;re testing too much. This should only be a 
    // unit test, not an integration test.
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
            var response = (OkObjectResult)await endpoint.AddIncident(input, httpRequest.Request);

            // Assert
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async void GetAllAddressByQueryReturnMatchingAddressList()
        {
            static Dictionary<string, StringValues> CreateDictionary(string key, string value)
            {
                var qs = new Dictionary<string, StringValues>
                {
                    { key, value }
                };
                return qs;
            }

            static async IAsyncEnumerable<Address> ReturnAsyncEnumerable()
            {
                await Task.CompletedTask;
                yield return new Address { Id = "gfm-123" };
            }

            // Arrange
            var contactRepositoryMock = new Mock<IContactRepository>();
            var incidentRepositoryMock = new Mock<IIncidentRepository>();
            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock
                .Setup(s => s.GetBySearchQueryAsync(It.IsAny<string>(), It.IsAny<INavigation>()))
                .Returns(ReturnAsyncEnumerable());

            var incidentUseCase = new IncidentUseCase(contactRepositoryMock.Object, incidentRepositoryMock.Object);
            var geocoderUseCase = new GeocoderUseCase(addressRepositoryMock.Object);

            var httpRequest = new DefaultHttpContext();
            httpRequest.Request.Query = new QueryCollection(CreateDictionary("query", "query"));
            var endpoint = new HttpPortalEndpoint(incidentUseCase, geocoderUseCase);

            // Act
            var response = (OkObjectResult)await endpoint.GetAddress(httpRequest.Request);
            var returnObject = (AddressOutputViewModel)response.Value;

            // Assert
            Assert.Equal(200, response.StatusCode);
            Assert.StartsWith("gfm-", returnObject.Address);
        }
    }
}
