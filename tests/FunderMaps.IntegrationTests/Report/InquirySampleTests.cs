using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using FunderMaps.IntegrationTests.Extensions;
using FunderMaps.IntegrationTests.Faker;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.IntegrationTests.Report
{
    public class InquirySampleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public InquirySampleTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        internal class FakeInquirySampleDtoData : EnumerableHelper<InquirySampleDto>
        {
            protected override IEnumerable<InquirySampleDto> GetEnumerableEntity()
            {
                return new InquirySampleDtoFaker().Generate(10, 100);
            }

            protected override IEnumerable<object[]> GetData()
            {
                var inquiry = new InquiryFaker().RuleFor(f => f.AuditStatus, f => AuditStatus.Pending); // TODO; can have more stats
                return GetEnumerableEntity().Select(s => new object[] { inquiry.Generate(), s });
            }
        }

        internal class FakeInquirySampleData : EnumerableHelper<InquirySample>
        {
            protected override IEnumerable<InquirySample> GetEnumerableEntity()
            {
                return new InquirySampleFaker().Generate(10, 100);
            }

            protected override IEnumerable<object[]> GetData()
            {
                var inquiry = new InquiryFaker().RuleFor(f => f.AuditStatus, f => AuditStatus.Pending); // TODO; can have more stats
                return GetEnumerableEntity().Select(s =>
                {
                    var i = inquiry.Generate();
                    s.Inquiry = i.Id;
                    return new object[] { i, s };
                });
            }
        }

        [Theory]
        [ClassData(typeof(FakeInquirySampleDtoData))]
        public async Task CreateInquirySampleReturnInquirySample(Inquiry inquiry, InquirySampleDto sample)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(inquiry)
                .CreateClient();
            var inquiryDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();
            var inquirySampleDataStore = _factory.Services.GetService<EntityDataStore<Inquiry>>();

            // Act
            var response = await client.PostAsJsonAsync($"api/inquiry/{inquiry.Id}/sample", sample).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(AuditStatus.Pending, inquiryDataStore.Entities[0].AuditStatus);
            Assert.True(inquirySampleDataStore.IsSet);
            var actualInquirySample = await response.Content.ReadFromJsonAsync<InquirySampleDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(sample.Address, actualInquirySample.Address);
            Assert.Equal(sample.Note, actualInquirySample.Note);
            Assert.Equal(sample.BuiltYear, actualInquirySample.BuiltYear);
            Assert.Equal(sample.Substructure, actualInquirySample.Substructure);
            Assert.Equal(sample.Cpt, actualInquirySample.Cpt);
            Assert.Equal(sample.MonitoringWell, actualInquirySample.MonitoringWell);
            Assert.Equal(sample.GroundwaterLevelTemp, actualInquirySample.GroundwaterLevelTemp);
            Assert.Equal(sample.GroundLevel, actualInquirySample.GroundLevel);
            Assert.Equal(sample.GroundwaterLevelNet, actualInquirySample.GroundwaterLevelNet);
            Assert.Equal(sample.Type, actualInquirySample.Type);
            Assert.Equal(sample.EnforcementTerm, actualInquirySample.EnforcementTerm);
            Assert.Equal(sample.RecoveryAdvised, actualInquirySample.RecoveryAdvised);
            Assert.Equal(sample.DamageCause, actualInquirySample.DamageCause);
            Assert.Equal(sample.DamageCharacteristics, actualInquirySample.DamageCharacteristics);
            Assert.Equal(sample.ConstructionPile, actualInquirySample.ConstructionPile);
            Assert.Equal(sample.WoodType, actualInquirySample.WoodType);
            Assert.Equal(sample.WoodEncroachement, actualInquirySample.WoodEncroachement);
            Assert.Equal(sample.ConstructionLevel, actualInquirySample.ConstructionLevel);
            Assert.Equal(sample.WoodLevel, actualInquirySample.WoodLevel);
            Assert.Equal(sample.PileDiameterTop, actualInquirySample.PileDiameterTop);
            Assert.Equal(sample.PileDiameterBottom, actualInquirySample.PileDiameterBottom);
            Assert.Equal(sample.PileHeadLevel, actualInquirySample.PileHeadLevel);
            Assert.Equal(sample.PileTipLevel, actualInquirySample.PileTipLevel);
            Assert.Equal(sample.FoundationDepth, actualInquirySample.FoundationDepth);
            Assert.Equal(sample.MasonLevel, actualInquirySample.MasonLevel);
            Assert.Equal(sample.ConcreteChargerLength, actualInquirySample.ConcreteChargerLength);
            Assert.Equal(sample.PileDistanceLength, actualInquirySample.PileDistanceLength);
            Assert.Equal(sample.WoodPenetrationDepth, actualInquirySample.WoodPenetrationDepth);
            Assert.Equal(sample.OverallQuality, actualInquirySample.OverallQuality);
            Assert.Equal(sample.WoodQuality, actualInquirySample.WoodQuality);
            Assert.Equal(sample.ConstructionQuality, actualInquirySample.ConstructionQuality);
            Assert.Equal(sample.WoodCapacityHorizontalQuality, actualInquirySample.WoodCapacityHorizontalQuality);
            Assert.Equal(sample.PileWoodCapacityVerticalQuality, actualInquirySample.PileWoodCapacityVerticalQuality);
            Assert.Equal(sample.CarryingCapacityQuality, actualInquirySample.CarryingCapacityQuality);
            Assert.Equal(sample.MasonQuality, actualInquirySample.MasonQuality);
            Assert.Equal(sample.WoodQualityNecessity, actualInquirySample.WoodQualityNecessity);
            Assert.Equal(sample.CrackIndoorRestored, actualInquirySample.CrackIndoorRestored);
            Assert.Equal(sample.CrackIndoorType, actualInquirySample.CrackIndoorType);
            Assert.Equal(sample.CrackIndoorSize, actualInquirySample.CrackIndoorSize);
            Assert.Equal(sample.CrackFacadeFrontRestored, actualInquirySample.CrackFacadeFrontRestored);
            Assert.Equal(sample.CrackFacadeFrontType, actualInquirySample.CrackFacadeFrontType);
            Assert.Equal(sample.CrackFacadeFrontSize, actualInquirySample.CrackFacadeFrontSize);
            Assert.Equal(sample.CrackFacadeBackRestored, actualInquirySample.CrackFacadeBackRestored);
            Assert.Equal(sample.CrackFacadeBackType, actualInquirySample.CrackFacadeBackType);
            Assert.Equal(sample.CrackFacadeBackSize, actualInquirySample.CrackFacadeBackSize);
            Assert.Equal(sample.CrackFacadeLeftRestored, actualInquirySample.CrackFacadeLeftRestored);
            Assert.Equal(sample.CrackFacadeLeftType, actualInquirySample.CrackFacadeLeftType);
            Assert.Equal(sample.CrackFacadeLeftSize, actualInquirySample.CrackFacadeLeftSize);
            Assert.Equal(sample.CrackFacadeRightRestored, actualInquirySample.CrackFacadeRightRestored);
            Assert.Equal(sample.CrackFacadeRightType, actualInquirySample.CrackFacadeRightType);
            Assert.Equal(sample.CrackFacadeRightSize, actualInquirySample.CrackFacadeRightSize);
            Assert.Equal(sample.DeformedFacade, actualInquirySample.DeformedFacade);
            Assert.Equal(sample.ThresholdUpdownSkewed, actualInquirySample.ThresholdUpdownSkewed);
            Assert.Equal(sample.ThresholdFrontLevel, actualInquirySample.ThresholdFrontLevel);
            Assert.Equal(sample.ThresholdBackLevel, actualInquirySample.ThresholdBackLevel);
            Assert.Equal(sample.SkewedParallel, actualInquirySample.SkewedParallel);
            Assert.Equal(sample.SkewedPerpendicular, actualInquirySample.SkewedPerpendicular);
            Assert.Equal(sample.SkewedFacade, actualInquirySample.SkewedFacade);
            Assert.Equal(sample.SettlementSpeed, actualInquirySample.SettlementSpeed);
        }

        [Theory]
        [ClassData(typeof(FakeInquirySampleData))]
        public async Task GetInquirySampleByIdReturnSingleInquirySample(Inquiry inquiry, InquirySample sample)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(sample)
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var actualInquirySample = await response.Content.ReadFromJsonAsync<InquirySampleDto>().ConfigureAwait(false);

            // Assert
            Assert.Equal(sample.Id, actualInquirySample.Id);
            Assert.Equal(sample.Address, actualInquirySample.Address);
            Assert.Equal(sample.Note, actualInquirySample.Note);
            Assert.Equal(sample.BuiltYear, actualInquirySample.BuiltYear);
            Assert.Equal(sample.Substructure, actualInquirySample.Substructure);
            Assert.Equal(sample.Cpt, actualInquirySample.Cpt);
            Assert.Equal(sample.MonitoringWell, actualInquirySample.MonitoringWell);
            Assert.Equal(sample.GroundwaterLevelTemp, actualInquirySample.GroundwaterLevelTemp);
            Assert.Equal(sample.GroundLevel, actualInquirySample.GroundLevel);
            Assert.Equal(sample.GroundwaterLevelNet, actualInquirySample.GroundwaterLevelNet);
            Assert.Equal(sample.Type, actualInquirySample.Type);
            Assert.Equal(sample.EnforcementTerm, actualInquirySample.EnforcementTerm);
            Assert.Equal(sample.RecoveryAdvised, actualInquirySample.RecoveryAdvised);
            Assert.Equal(sample.DamageCause, actualInquirySample.DamageCause);
            Assert.Equal(sample.DamageCharacteristics, actualInquirySample.DamageCharacteristics);
            Assert.Equal(sample.ConstructionPile, actualInquirySample.ConstructionPile);
            Assert.Equal(sample.WoodType, actualInquirySample.WoodType);
            Assert.Equal(sample.WoodEncroachement, actualInquirySample.WoodEncroachement);
            Assert.Equal(sample.ConstructionLevel, actualInquirySample.ConstructionLevel);
            Assert.Equal(sample.WoodLevel, actualInquirySample.WoodLevel);
            Assert.Equal(sample.PileDiameterTop, actualInquirySample.PileDiameterTop);
            Assert.Equal(sample.PileDiameterBottom, actualInquirySample.PileDiameterBottom);
            Assert.Equal(sample.PileHeadLevel, actualInquirySample.PileHeadLevel);
            Assert.Equal(sample.PileTipLevel, actualInquirySample.PileTipLevel);
            Assert.Equal(sample.FoundationDepth, actualInquirySample.FoundationDepth);
            Assert.Equal(sample.MasonLevel, actualInquirySample.MasonLevel);
            Assert.Equal(sample.ConcreteChargerLength, actualInquirySample.ConcreteChargerLength);
            Assert.Equal(sample.PileDistanceLength, actualInquirySample.PileDistanceLength);
            Assert.Equal(sample.WoodPenetrationDepth, actualInquirySample.WoodPenetrationDepth);
            Assert.Equal(sample.OverallQuality, actualInquirySample.OverallQuality);
            Assert.Equal(sample.WoodQuality, actualInquirySample.WoodQuality);
            Assert.Equal(sample.ConstructionQuality, actualInquirySample.ConstructionQuality);
            Assert.Equal(sample.WoodCapacityHorizontalQuality, actualInquirySample.WoodCapacityHorizontalQuality);
            Assert.Equal(sample.PileWoodCapacityVerticalQuality, actualInquirySample.PileWoodCapacityVerticalQuality);
            Assert.Equal(sample.CarryingCapacityQuality, actualInquirySample.CarryingCapacityQuality);
            Assert.Equal(sample.MasonQuality, actualInquirySample.MasonQuality);
            Assert.Equal(sample.WoodQualityNecessity, actualInquirySample.WoodQualityNecessity);
            Assert.Equal(sample.CrackIndoorRestored, actualInquirySample.CrackIndoorRestored);
            Assert.Equal(sample.CrackIndoorType, actualInquirySample.CrackIndoorType);
            Assert.Equal(sample.CrackIndoorSize, actualInquirySample.CrackIndoorSize);
            Assert.Equal(sample.CrackFacadeFrontRestored, actualInquirySample.CrackFacadeFrontRestored);
            Assert.Equal(sample.CrackFacadeFrontType, actualInquirySample.CrackFacadeFrontType);
            Assert.Equal(sample.CrackFacadeFrontSize, actualInquirySample.CrackFacadeFrontSize);
            Assert.Equal(sample.CrackFacadeBackRestored, actualInquirySample.CrackFacadeBackRestored);
            Assert.Equal(sample.CrackFacadeBackType, actualInquirySample.CrackFacadeBackType);
            Assert.Equal(sample.CrackFacadeBackSize, actualInquirySample.CrackFacadeBackSize);
            Assert.Equal(sample.CrackFacadeLeftRestored, actualInquirySample.CrackFacadeLeftRestored);
            Assert.Equal(sample.CrackFacadeLeftType, actualInquirySample.CrackFacadeLeftType);
            Assert.Equal(sample.CrackFacadeLeftSize, actualInquirySample.CrackFacadeLeftSize);
            Assert.Equal(sample.CrackFacadeRightRestored, actualInquirySample.CrackFacadeRightRestored);
            Assert.Equal(sample.CrackFacadeRightType, actualInquirySample.CrackFacadeRightType);
            Assert.Equal(sample.CrackFacadeRightSize, actualInquirySample.CrackFacadeRightSize);
            Assert.Equal(sample.DeformedFacade, actualInquirySample.DeformedFacade);
            Assert.Equal(sample.ThresholdUpdownSkewed, actualInquirySample.ThresholdUpdownSkewed);
            Assert.Equal(sample.ThresholdFrontLevel, actualInquirySample.ThresholdFrontLevel);
            Assert.Equal(sample.ThresholdBackLevel, actualInquirySample.ThresholdBackLevel);
            Assert.Equal(sample.SkewedParallel, actualInquirySample.SkewedParallel);
            Assert.Equal(sample.SkewedPerpendicular, actualInquirySample.SkewedPerpendicular);
            Assert.Equal(sample.SkewedFacade, actualInquirySample.SkewedFacade);
            Assert.Equal(sample.SettlementSpeed, actualInquirySample.SettlementSpeed);
        }

        [Fact]
        public async Task GetAllInquirySampleReturnPageInquiry()
        {
            // Arrange
            var inquiry = new InquiryFaker().Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(new InquirySampleFaker().RuleFor(f => f.Inquiry, f => inquiry.Id).Generate(10, 100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var inquirySampleList = await response.Content.ReadFromJsonAsync<List<InquirySampleDto>>().ConfigureAwait(false);
            Assert.NotNull(inquirySampleList);

            // Assert
            Assert.True(inquirySampleList.Count > 0);
        }

        [Fact]
        public async Task GetAllInquirySampleReturnAllInquiry()
        {
            // Arrange
            var inquiry = new InquiryFaker().Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(new InquirySampleFaker().RuleFor(f => f.Inquiry, f => inquiry.Id).Generate(100))
                .CreateClient();

            // Act
            var response = await client.GetAsync($"api/inquiry/{inquiry.Id}/sample?limit=100").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var inquirySampleList = await response.Content.ReadFromJsonAsync<List<InquirySampleDto>>().ConfigureAwait(false);
            Assert.NotNull(inquirySampleList);

            // Assert
            Assert.Equal(100, inquirySampleList.Count);
        }

        [Theory]
        [ClassData(typeof(FakeInquirySampleData))]
        public async Task UpdateInquirySampleReturnNoContent(Inquiry inquiry, InquirySample sample)
        {
            // Arrange
            var newInquirySample = new InquirySampleFaker().Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(sample)
                .CreateClient();
            var inquirySampleDataStore = _factory.Services.GetService<EntityDataStore<InquirySample>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", newInquirySample).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, inquirySampleDataStore.Entities.Count);

            // Assert
            var actualInquirySample = inquirySampleDataStore.Entities[0];
            Assert.Equal(sample.Id, actualInquirySample.Id);
            Assert.Equal(newInquirySample.Address, actualInquirySample.Address);
            Assert.Equal(newInquirySample.Note, actualInquirySample.Note);
            Assert.Equal(newInquirySample.BuiltYear, actualInquirySample.BuiltYear);
            Assert.Equal(newInquirySample.Substructure, actualInquirySample.Substructure);
            Assert.Equal(newInquirySample.Cpt, actualInquirySample.Cpt);
            Assert.Equal(newInquirySample.MonitoringWell, actualInquirySample.MonitoringWell);
            Assert.Equal(newInquirySample.GroundwaterLevelTemp, actualInquirySample.GroundwaterLevelTemp);
            Assert.Equal(newInquirySample.GroundLevel, actualInquirySample.GroundLevel);
            Assert.Equal(newInquirySample.GroundwaterLevelNet, actualInquirySample.GroundwaterLevelNet);
            Assert.Equal(newInquirySample.Type, actualInquirySample.Type);
            Assert.Equal(newInquirySample.EnforcementTerm, actualInquirySample.EnforcementTerm);
            Assert.Equal(newInquirySample.RecoveryAdvised, actualInquirySample.RecoveryAdvised);
            Assert.Equal(newInquirySample.DamageCause, actualInquirySample.DamageCause);
            Assert.Equal(newInquirySample.DamageCharacteristics, actualInquirySample.DamageCharacteristics);
            Assert.Equal(newInquirySample.ConstructionPile, actualInquirySample.ConstructionPile);
            Assert.Equal(newInquirySample.WoodType, actualInquirySample.WoodType);
            Assert.Equal(newInquirySample.WoodEncroachement, actualInquirySample.WoodEncroachement);
            Assert.Equal(newInquirySample.ConstructionLevel, actualInquirySample.ConstructionLevel);
            Assert.Equal(newInquirySample.WoodLevel, actualInquirySample.WoodLevel);
            Assert.Equal(newInquirySample.PileDiameterTop, actualInquirySample.PileDiameterTop);
            Assert.Equal(newInquirySample.PileDiameterBottom, actualInquirySample.PileDiameterBottom);
            Assert.Equal(newInquirySample.PileHeadLevel, actualInquirySample.PileHeadLevel);
            Assert.Equal(newInquirySample.PileTipLevel, actualInquirySample.PileTipLevel);
            Assert.Equal(newInquirySample.FoundationDepth, actualInquirySample.FoundationDepth);
            Assert.Equal(newInquirySample.MasonLevel, actualInquirySample.MasonLevel);
            Assert.Equal(newInquirySample.ConcreteChargerLength, actualInquirySample.ConcreteChargerLength);
            Assert.Equal(newInquirySample.PileDistanceLength, actualInquirySample.PileDistanceLength);
            Assert.Equal(newInquirySample.WoodPenetrationDepth, actualInquirySample.WoodPenetrationDepth);
            Assert.Equal(newInquirySample.OverallQuality, actualInquirySample.OverallQuality);
            Assert.Equal(newInquirySample.WoodQuality, actualInquirySample.WoodQuality);
            Assert.Equal(newInquirySample.ConstructionQuality, actualInquirySample.ConstructionQuality);
            Assert.Equal(newInquirySample.WoodCapacityHorizontalQuality, actualInquirySample.WoodCapacityHorizontalQuality);
            Assert.Equal(newInquirySample.PileWoodCapacityVerticalQuality, actualInquirySample.PileWoodCapacityVerticalQuality);
            Assert.Equal(newInquirySample.CarryingCapacityQuality, actualInquirySample.CarryingCapacityQuality);
            Assert.Equal(newInquirySample.MasonQuality, actualInquirySample.MasonQuality);
            Assert.Equal(newInquirySample.WoodQualityNecessity, actualInquirySample.WoodQualityNecessity);
            Assert.Equal(newInquirySample.CrackIndoorRestored, actualInquirySample.CrackIndoorRestored);
            Assert.Equal(newInquirySample.CrackIndoorType, actualInquirySample.CrackIndoorType);
            Assert.Equal(newInquirySample.CrackIndoorSize, actualInquirySample.CrackIndoorSize);
            Assert.Equal(newInquirySample.CrackFacadeFrontRestored, actualInquirySample.CrackFacadeFrontRestored);
            Assert.Equal(newInquirySample.CrackFacadeFrontType, actualInquirySample.CrackFacadeFrontType);
            Assert.Equal(newInquirySample.CrackFacadeFrontSize, actualInquirySample.CrackFacadeFrontSize);
            Assert.Equal(newInquirySample.CrackFacadeBackRestored, actualInquirySample.CrackFacadeBackRestored);
            Assert.Equal(newInquirySample.CrackFacadeBackType, actualInquirySample.CrackFacadeBackType);
            Assert.Equal(newInquirySample.CrackFacadeBackSize, actualInquirySample.CrackFacadeBackSize);
            Assert.Equal(newInquirySample.CrackFacadeLeftRestored, actualInquirySample.CrackFacadeLeftRestored);
            Assert.Equal(newInquirySample.CrackFacadeLeftType, actualInquirySample.CrackFacadeLeftType);
            Assert.Equal(newInquirySample.CrackFacadeLeftSize, actualInquirySample.CrackFacadeLeftSize);
            Assert.Equal(newInquirySample.CrackFacadeRightRestored, actualInquirySample.CrackFacadeRightRestored);
            Assert.Equal(newInquirySample.CrackFacadeRightType, actualInquirySample.CrackFacadeRightType);
            Assert.Equal(newInquirySample.CrackFacadeRightSize, actualInquirySample.CrackFacadeRightSize);
            Assert.Equal(newInquirySample.DeformedFacade, actualInquirySample.DeformedFacade);
            Assert.Equal(newInquirySample.ThresholdUpdownSkewed, actualInquirySample.ThresholdUpdownSkewed);
            Assert.Equal(newInquirySample.ThresholdFrontLevel, actualInquirySample.ThresholdFrontLevel);
            Assert.Equal(newInquirySample.ThresholdBackLevel, actualInquirySample.ThresholdBackLevel);
            Assert.Equal(newInquirySample.SkewedParallel, actualInquirySample.SkewedParallel);
            Assert.Equal(newInquirySample.SkewedPerpendicular, actualInquirySample.SkewedPerpendicular);
            Assert.Equal(newInquirySample.SkewedFacade, actualInquirySample.SkewedFacade);
            Assert.Equal(newInquirySample.SettlementSpeed, actualInquirySample.SettlementSpeed);
        }

        [Theory]
        [ClassData(typeof(FakeInquirySampleData))]
        public async Task DeleteInquirySampleReturnNoContent(Inquiry inquiry, InquirySample sample)
        {
            // Arrange
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(sample)
                .CreateClient();
            var inquirySampleDataStore = _factory.Services.GetService<EntityDataStore<InquirySample>>();

            // Act
            var response = await client.DeleteAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Assert
            Assert.False(inquirySampleDataStore.IsSet);
        }
    }
}
