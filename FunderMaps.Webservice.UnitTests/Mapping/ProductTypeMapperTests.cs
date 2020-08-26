using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using System;
using Xunit;

namespace FunderMaps.Webservice.UnitTests.Mapping
{
    /// <summary>
    ///     Test class for <see cref = "Webservice.ProductTypeMapper" />.
    /// </ summary >
    public sealed class ProductTypeMapperTests
    {
        [Theory]
        [InlineData(AnalysisProductTypeResponseModel.BuildingData, AnalysisProductType.BuildingData)]
        [InlineData(AnalysisProductTypeResponseModel.Complete, AnalysisProductType.Complete)]
        [InlineData(AnalysisProductTypeResponseModel.Costs, AnalysisProductType.Costs)]
        [InlineData(AnalysisProductTypeResponseModel.BuildingDescription, AnalysisProductType.BuildingDescription)]
        [InlineData(AnalysisProductTypeResponseModel.Foundation, AnalysisProductType.Foundation)]
        [InlineData(AnalysisProductTypeResponseModel.FoundationPlus, AnalysisProductType.FoundationPlus)]
        [InlineData(AnalysisProductTypeResponseModel.Risk, AnalysisProductType.Risk)]
        public void AnalysisProductTypeMaps(AnalysisProductTypeResponseModel value, AnalysisProductType expected) =>
            Assert.Equal(expected, ProductTypeMapper.MapAnalysis(value));

        [Theory]
        [InlineData(StatisticsProductTypeResponseModel.BuildingsRestored, StatisticsProductType.BuildingsRestored)]
        [InlineData(StatisticsProductTypeResponseModel.ConstructionYears, StatisticsProductType.ConstructionYears)]
        [InlineData(StatisticsProductTypeResponseModel.DataCollected, StatisticsProductType.DataCollected)]
        [InlineData(StatisticsProductTypeResponseModel.FoundationRatio, StatisticsProductType.FoundationRatio)]
        [InlineData(StatisticsProductTypeResponseModel.FoundationRisk, StatisticsProductType.FoundationRisk)]
        [InlineData(StatisticsProductTypeResponseModel.Incidents, StatisticsProductType.Incidents)]
        [InlineData(StatisticsProductTypeResponseModel.Reports, StatisticsProductType.Reports)]
        public void StatisticsProductTypeMaps(StatisticsProductTypeResponseModel value, StatisticsProductType expected) =>
            Assert.Equal(expected, ProductTypeMapper.MapStatistics(value));

        [Theory]
        [InlineData(AnalysisProductType.BuildingData, typeof(AnalysisBuildingDataResponseModel))]
        [InlineData(AnalysisProductType.BuildingDescription, typeof(AnalysisBuildingDescriptionResponseModel))]
        [InlineData(AnalysisProductType.Complete, typeof(AnalysisCompleteResponseModel))]
        [InlineData(AnalysisProductType.Costs, typeof(AnalysisCostsResponseModel))]
        [InlineData(AnalysisProductType.Foundation, typeof(AnalysisFoundationResponseModel))]
        [InlineData(AnalysisProductType.FoundationPlus, typeof(AnalysisFoundationPlusResponseModel))]
        [InlineData(AnalysisProductType.Risk, typeof(AnalysisRiskResponseModel))]
        public void AnalysisResponseModelTypeFromProduct(AnalysisProductType value, Type expected) =>
            Assert.Equal(expected, ProductTypeMapper.MapAnalysisResponseModelType(value));

        [Fact]
        public void InvalidAnalysisProductTypeThrows()
            => Assert.Throws<InvalidOperationException>(() => ProductTypeMapper.MapAnalysis((AnalysisProductTypeResponseModel)453));

        [Fact]
        public void InvalidStatisticProductTypeThrows()
            => Assert.Throws<InvalidOperationException>(() => ProductTypeMapper.MapStatistics((StatisticsProductTypeResponseModel)485));
    }
}
