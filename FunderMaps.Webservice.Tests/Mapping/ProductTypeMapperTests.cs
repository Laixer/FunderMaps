using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.Mapping;
using FunderMaps.Webservice.ResponseModels.Analysis;
using System;
using Xunit;

namespace FunderMaps.Webservice.Tests.Mapping
{
    /// <summary>
    ///     Test class for <see cref="Webservice.ProductTypeMapper"/>
    /// </summary>
    public sealed class ProductTypeMapperTests
    {
        [Theory]
        [InlineData("BUILDINGDATA", AnalysisProductType.BuildingData)]
        [InlineData("COMPLETE", AnalysisProductType.Complete)]
        [InlineData("COSTS", AnalysisProductType.Costs)]
        [InlineData("DESCRIPTION", AnalysisProductType.BuildingDescription)]
        [InlineData("FOUNDATION", AnalysisProductType.Foundation)]
        [InlineData("FOUNDATIONPLUS", AnalysisProductType.FoundationPlus)]
        [InlineData("RISK", AnalysisProductType.Risk)]
        public void AnalysisProductTypeFromUpperString(string value, AnalysisProductType expected) =>
            // Assert
            Assert.Equal(expected, ProductTypeMapper.MapAnalysisFromString(value));

        [Theory]
        [InlineData("buildingdata", AnalysisProductType.BuildingData)]
        [InlineData("complete", AnalysisProductType.Complete)]
        [InlineData("costs", AnalysisProductType.Costs)]
        [InlineData("description", AnalysisProductType.BuildingDescription)]
        [InlineData("foundation", AnalysisProductType.Foundation)]
        [InlineData("foundationplus", AnalysisProductType.FoundationPlus)]
        [InlineData("risk", AnalysisProductType.Risk)]
        public void AnalysisProductTypeFromLowerString(string value, AnalysisProductType expected) =>
            // Assert
            Assert.Equal(expected, ProductTypeMapper.MapAnalysisFromString(value));

        [Theory]
        [InlineData("RESTORATION", StatisticsProductType.BuildingsRestored)]
        [InlineData("CONSTRUCTIONYEARS", StatisticsProductType.ConstructionYears)]
        [InlineData("DATACOLLECTED", StatisticsProductType.DataCollected)]
        [InlineData("FOUNDATIONRATIO", StatisticsProductType.FoundationRatio)]
        [InlineData("FOUNDATIONRISK", StatisticsProductType.FoundationRisk)]
        [InlineData("INCIDENTS", StatisticsProductType.Incidents)]
        [InlineData("REPORTS", StatisticsProductType.Reports)]
        public void StatisticsProductTypeFromUpperString(string value, StatisticsProductType expected) =>
            // Assert
            Assert.Equal(expected, ProductTypeMapper.MapStatisticsFromString(value));

        [Theory]
        [InlineData("restoration", StatisticsProductType.BuildingsRestored)]
        [InlineData("constructionyears", StatisticsProductType.ConstructionYears)]
        [InlineData("datacollected", StatisticsProductType.DataCollected)]
        [InlineData("foundationratio", StatisticsProductType.FoundationRatio)]
        [InlineData("foundationrisk", StatisticsProductType.FoundationRisk)]
        [InlineData("incidents", StatisticsProductType.Incidents)]
        [InlineData("reports", StatisticsProductType.Reports)]
        public void StatisticsProductTypeFromLowerString(string value, StatisticsProductType expected) =>
            // Assert
            Assert.Equal(expected, ProductTypeMapper.MapStatisticsFromString(value));

        [Theory]
        [InlineData(AnalysisProductType.BuildingData, typeof(AnalysisBuildingDataResponseModel))]
        [InlineData(AnalysisProductType.BuildingDescription, typeof(AnalysisBuildingDescriptionResponseModel))]
        [InlineData(AnalysisProductType.Complete, typeof(AnalysisCompleteResponseModel))]
        [InlineData(AnalysisProductType.Costs, typeof(AnalysisCostsResponseModel))]
        [InlineData(AnalysisProductType.Foundation, typeof(AnalysisFoundationResponseModel))]
        [InlineData(AnalysisProductType.FoundationPlus, typeof(AnalysisFoundationPlusResponseModel))]
        [InlineData(AnalysisProductType.Risk, typeof(AnalysisRiskResponseModel))]
        public void AnalysisResponseModelTypeFromProduct(AnalysisProductType value, Type expected)
        {
            // Assert
            Assert.Equal(expected, ProductTypeMapper.MapAnalysisResponseModelType(value));
        }

        [Fact]
        public void AnalysisProductTypeFromStringThrows()
        {
            // Arrange
            var str = "asdflkjsdhfa"; // TODO Make randomly generate

            // Assert
            Assert.Throws<ProductNotFoundException>(() => ProductTypeMapper.MapAnalysisFromString(str));
        }

        [Fact]
        public void StatisticProductTypeFromStringThrows()
        {
            // Arrange
            var str = "lkkjyrtur"; // TODO Make randomly generate

            // Assert
            Assert.Throws<ProductNotFoundException>(() => ProductTypeMapper.MapStatisticsFromString(str));
        }
    }
}
