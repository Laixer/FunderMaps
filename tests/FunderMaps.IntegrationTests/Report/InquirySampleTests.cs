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
                return new InquirySampleDtoFaker().Generate(10, 1000);
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
                return new InquirySampleFaker().Generate(10, 1000);
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
    }
}
