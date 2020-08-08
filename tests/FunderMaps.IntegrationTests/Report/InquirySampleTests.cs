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
            var newSample = new InquirySampleFaker().Generate();
            var client = _factory
                .WithDataStoreList(inquiry)
                .WithDataStoreList(sample)
                .CreateClient();
            var inquirySampleDataStore = _factory.Services.GetService<EntityDataStore<InquirySample>>();

            // Act
            var response = await client.PutAsJsonAsync($"api/inquiry/{inquiry.Id}/sample/{sample.Id}", newSample).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(1, inquirySampleDataStore.Entities.Count);

            // Assert
            var actualInquirySample = inquirySampleDataStore.Entities[0];
            Assert.Equal(sample.Id, actualInquirySample.Id);
            Assert.Equal(newSample.Address, actualInquirySample.Address);
            Assert.Equal(newSample.Note, actualInquirySample.Note);
            Assert.Equal(newSample.BuiltYear, actualInquirySample.BuiltYear);
            Assert.Equal(newSample.Substructure, actualInquirySample.Substructure);
            Assert.Equal(newSample.Cpt, actualInquirySample.Cpt);
            Assert.Equal(newSample.MonitoringWell, actualInquirySample.MonitoringWell);
            Assert.Equal(newSample.GroundwaterLevelTemp, actualInquirySample.GroundwaterLevelTemp);
            Assert.Equal(newSample.GroundLevel, actualInquirySample.GroundLevel);
            Assert.Equal(newSample.GroundwaterLevelNet, actualInquirySample.GroundwaterLevelNet);
            Assert.Equal(newSample.Type, actualInquirySample.Type);
            Assert.Equal(newSample.EnforcementTerm, actualInquirySample.EnforcementTerm);
            Assert.Equal(newSample.RecoveryAdvised, actualInquirySample.RecoveryAdvised);
            Assert.Equal(newSample.DamageCause, actualInquirySample.DamageCause);
            Assert.Equal(newSample.DamageCharacteristics, actualInquirySample.DamageCharacteristics);
            Assert.Equal(newSample.ConstructionPile, actualInquirySample.ConstructionPile);
            Assert.Equal(newSample.WoodType, actualInquirySample.WoodType);
            Assert.Equal(newSample.WoodEncroachement, actualInquirySample.WoodEncroachement);
            Assert.Equal(newSample.ConstructionLevel, actualInquirySample.ConstructionLevel);
            Assert.Equal(newSample.WoodLevel, actualInquirySample.WoodLevel);
            Assert.Equal(newSample.PileDiameterTop, actualInquirySample.PileDiameterTop);
            Assert.Equal(newSample.PileDiameterBottom, actualInquirySample.PileDiameterBottom);
            Assert.Equal(newSample.PileHeadLevel, actualInquirySample.PileHeadLevel);
            Assert.Equal(newSample.PileTipLevel, actualInquirySample.PileTipLevel);
            Assert.Equal(newSample.FoundationDepth, actualInquirySample.FoundationDepth);
            Assert.Equal(newSample.MasonLevel, actualInquirySample.MasonLevel);
            Assert.Equal(newSample.ConcreteChargerLength, actualInquirySample.ConcreteChargerLength);
            Assert.Equal(newSample.PileDistanceLength, actualInquirySample.PileDistanceLength);
            Assert.Equal(newSample.WoodPenetrationDepth, actualInquirySample.WoodPenetrationDepth);
            Assert.Equal(newSample.OverallQuality, actualInquirySample.OverallQuality);
            Assert.Equal(newSample.WoodQuality, actualInquirySample.WoodQuality);
            Assert.Equal(newSample.ConstructionQuality, actualInquirySample.ConstructionQuality);
            Assert.Equal(newSample.WoodCapacityHorizontalQuality, actualInquirySample.WoodCapacityHorizontalQuality);
            Assert.Equal(newSample.PileWoodCapacityVerticalQuality, actualInquirySample.PileWoodCapacityVerticalQuality);
            Assert.Equal(newSample.CarryingCapacityQuality, actualInquirySample.CarryingCapacityQuality);
            Assert.Equal(newSample.MasonQuality, actualInquirySample.MasonQuality);
            Assert.Equal(newSample.WoodQualityNecessity, actualInquirySample.WoodQualityNecessity);
            Assert.Equal(newSample.CrackIndoorRestored, actualInquirySample.CrackIndoorRestored);
            Assert.Equal(newSample.CrackIndoorType, actualInquirySample.CrackIndoorType);
            Assert.Equal(newSample.CrackIndoorSize, actualInquirySample.CrackIndoorSize);
            Assert.Equal(newSample.CrackFacadeFrontRestored, actualInquirySample.CrackFacadeFrontRestored);
            Assert.Equal(newSample.CrackFacadeFrontType, actualInquirySample.CrackFacadeFrontType);
            Assert.Equal(newSample.CrackFacadeFrontSize, actualInquirySample.CrackFacadeFrontSize);
            Assert.Equal(newSample.CrackFacadeBackRestored, actualInquirySample.CrackFacadeBackRestored);
            Assert.Equal(newSample.CrackFacadeBackType, actualInquirySample.CrackFacadeBackType);
            Assert.Equal(newSample.CrackFacadeBackSize, actualInquirySample.CrackFacadeBackSize);
            Assert.Equal(newSample.CrackFacadeLeftRestored, actualInquirySample.CrackFacadeLeftRestored);
            Assert.Equal(newSample.CrackFacadeLeftType, actualInquirySample.CrackFacadeLeftType);
            Assert.Equal(newSample.CrackFacadeLeftSize, actualInquirySample.CrackFacadeLeftSize);
            Assert.Equal(newSample.CrackFacadeRightRestored, actualInquirySample.CrackFacadeRightRestored);
            Assert.Equal(newSample.CrackFacadeRightType, actualInquirySample.CrackFacadeRightType);
            Assert.Equal(newSample.CrackFacadeRightSize, actualInquirySample.CrackFacadeRightSize);
            Assert.Equal(newSample.DeformedFacade, actualInquirySample.DeformedFacade);
            Assert.Equal(newSample.ThresholdUpdownSkewed, actualInquirySample.ThresholdUpdownSkewed);
            Assert.Equal(newSample.ThresholdFrontLevel, actualInquirySample.ThresholdFrontLevel);
            Assert.Equal(newSample.ThresholdBackLevel, actualInquirySample.ThresholdBackLevel);
            Assert.Equal(newSample.SkewedParallel, actualInquirySample.SkewedParallel);
            Assert.Equal(newSample.SkewedPerpendicular, actualInquirySample.SkewedPerpendicular);
            Assert.Equal(newSample.SkewedFacade, actualInquirySample.SkewedFacade);
            Assert.Equal(newSample.SettlementSpeed, actualInquirySample.SettlementSpeed);
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
