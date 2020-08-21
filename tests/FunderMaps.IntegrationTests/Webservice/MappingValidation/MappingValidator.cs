using FunderMaps.Core.Types.Distributions;
using FunderMaps.Core.Types.Products;
using FunderMaps.Webservice.ResponseModels;
using FunderMaps.Webservice.ResponseModels.Analysis;
using FunderMaps.Webservice.ResponseModels.Types;
using System;
using System.Linq;
using Xunit;

namespace FunderMaps.IntegrationTests.Webservice.MappingValidation
{
    /// TODO Contains a lot of duplicate code.
    /// TODO External id and external source are not present in our analysis result. Do we want this?
    /// <summary>
    ///     Checks our mapping for analysis products.
    /// </summary>
    internal static class MappingValidator
    {
        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisBuildingData(AnalysisProduct product, AnalysisBuildingDataResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(product.BuildingHeight, model.BuildingHeight);
            Assert.Equal(product.ConstructionYear, model.ConstructionYear);
            Assert.Equal(TestEnumMapper.Map(product.FoundationType), model.FoundationType);
        }

        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisFoundation(AnalysisProduct product, AnalysisFoundationResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(product.DewateringDepth, model.DewateringDepth);
            Assert.Equal(product.Drystand, model.Drystand);
            Assert.Equal(TestEnumMapper.Map(product.FoundationRisk), model.FoundationRisk);
            Assert.Equal(TestEnumMapper.Map(product.FoundationType), model.FoundationType);
            Assert.Equal(product.GroundLevel, model.GroundLevel);
            Assert.Equal(product.GroundWaterLevel, model.GroundWaterLevel);

            // FUTURE Description service should be tested separately.
            // Assert.Equal(product.FullDescription, descriptionService.GenerateFullDescription(product));
        }

        /// TODO Inheritance with <see cref="ValidateAnalysisFoundation(AnalysisProduct, AnalysisFoundationResponseModel)"/>? Think not.
        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisFoundationPlus(AnalysisProduct product, AnalysisFoundationPlusResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(product.DataCollectedPercentage, model.DataCollectedPercentage);
            Assert.Equal(product.DewateringDepth, model.DewateringDepth);
            Assert.Equal(product.Drystand, model.Drystand);
            Assert.Equal(TestEnumMapper.Map(product.FoundationRisk), model.FoundationRisk);
            Assert.Equal(TestEnumMapper.Map(product.FoundationType), model.FoundationType);
            Assert.Equal(product.GroundLevel, model.GroundLevel);
            Assert.Equal(product.GroundWaterLevel, model.GroundWaterLevel);
            Assert.Equal(TestEnumMapper.Map(product.Reliability), model.Reliability);
            Assert.Equal(product.TotalReportCount, model.TotalReportCount);

            ValidateConstructionYearDistribution(product.ConstructionYearDistribution, model.ConstructionYearDistribution);
            ValidateFoundationRiskDistribution(product.FoundationRiskDistribution, model.FoundationRiskDistribution);
            ValidateFoundationTypeDistribution(product.FoundationTypeDistribution, model.FoundationTypeDistribution);

            // FUTURE Description service should be tested separately.
            // Assert.Equal(product.FullDescription, descriptionService.GenerateFullDescription(product));
            // Assert.Equal(product.TerrainDescription, descriptionService.GenerateTerrainDescription(product));
        }

        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisCosts(AnalysisProduct product, AnalysisCostsResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(TestEnumMapper.Map(product.FoundationRisk), model.FoundationRisk);
            Assert.Equal(TestEnumMapper.Map(product.Reliability), model.Reliability);
            Assert.Equal(product.RestorationCosts, model.RestorationCosts);
            Assert.Equal(product.TotalBuildingRestoredCount, model.TotalBuildingRestoredCount);
            Assert.Equal(product.TotalIncidentCount, model.TotalIncidentCount);
        }

        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisComplete(AnalysisProduct product, AnalysisCompleteResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(product.BuildingHeight, model.BuildingHeight);
            Assert.Equal(product.ConstructionYear, model.ConstructionYear);
            Assert.Equal(product.DataCollectedPercentage, model.DataCollectedPercentage);
            Assert.Equal(product.DewateringDepth, model.DewateringDepth);
            Assert.Equal(product.Drystand, model.Drystand);
            Assert.Equal(TestEnumMapper.Map(product.FoundationRisk), model.FoundationRisk);
            Assert.Equal(TestEnumMapper.Map(product.FoundationType), model.FoundationType);
            Assert.Equal(product.GroundLevel, model.GroundLevel);
            Assert.Equal(product.GroundWaterLevel, model.GroundWaterLevel);
            Assert.Equal(TestEnumMapper.Map(product.Reliability), model.Reliability);
            Assert.Equal(product.RestorationCosts, model.RestorationCosts);
            Assert.Equal(product.TotalBuildingRestoredCount, model.TotalBuildingRestoredCount);
            Assert.Equal(product.TotalIncidentCount, model.TotalIncidentCount);
            Assert.Equal(product.TotalReportCount, model.TotalReportCount);

            ValidateConstructionYearDistribution(product.ConstructionYearDistribution, model.ConstructionYearDistribution);
            ValidateFoundationRiskDistribution(product.FoundationRiskDistribution, model.FoundationRiskDistribution);
            ValidateFoundationTypeDistribution(product.FoundationTypeDistribution, model.FoundationTypeDistribution);

            // FUTURE Description service should be tested separately.
            // Assert.Equal(product.FullDescription, descriptionService.GenerateFullDescription(product));
            // Assert.Equal(product.TerrainDescription, descriptionService.GenerateTerrainDescription(product));
        }

        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisBuildingDescription(AnalysisProduct product, AnalysisBuildingDescriptionResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);

            // FUTURE Description service should be tested separately.
            // Assert.Equal(product.FullDescription, descriptionService.GenerateFullDescription(product));
            // Assert.Equal(product.TerrainDescription, descriptionService.GenerateTerrainDescription(product));
        }

        /// <summary>
        ///     Validate a <see cref="AnalysisCompleteResponseModel"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisCompleteResponseModel"/></param>
        internal static void ValidateAnalysisRisk(AnalysisProduct product, AnalysisRiskResponseModel model)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            ValidateBase(product, model);
            Assert.Equal(product.DewateringDepth, model.DewateringDepth);
            Assert.Equal(product.Drystand, model.Drystand);
            Assert.Equal(TestEnumMapper.Map(product.FoundationRisk), model.FoundationRisk);
            Assert.Equal(TestEnumMapper.Map(product.FoundationType), model.FoundationType);
            Assert.Equal(TestEnumMapper.Map(product.Reliability), model.Reliability);
            Assert.Equal(product.RestorationCosts, model.RestorationCosts);

            // FUTURE Description service should be tested separately.
            // Assert.Equal(product.FullDescription, descriptionService.GenerateFullDescription(product));
        }

        /// <summary>
        ///     Validate parameters which relate to the <see cref="AnalysisResponseModelBase"/>.
        /// </summary>
        /// <param name="product"><see cref="AnalysisProduct"/></param>
        /// <param name="model"><see cref="AnalysisResponseModelBase"/></param>
        private static void ValidateBase(AnalysisProduct product, AnalysisResponseModelBase model)
        {
            // Assert.
            Assert.Equal(product.Id, model.Id);
            Assert.Equal(product.NeighborhoodId, model.NeighborhoodId);
        }

        /// <summary>
        ///     Validate a <see cref="ConstructionYearDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="ConstructionYearDistribution"/></param>
        /// <param name="model"><see cref="ConstructionYearDistributionResponseModel"/></param>
        internal static void ValidateConstructionYearDistribution(ConstructionYearDistribution entity, ConstructionYearDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Else, do nullchecking.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            Assert.Equal(entity.Decades.Count(), model.Decades.Count());
            for (int i = 0; i < entity.Decades.Count(); i++)
            {
                var entityPair = entity.Decades.ToArray()[i];
                var modelPair = model.Decades.ToArray()[i];
                Assert.Equal(entityPair.Decade.YearFrom, modelPair.Decade.YearFrom);
                Assert.Equal(entityPair.Decade.YearTo, modelPair.Decade.YearTo);
                Assert.Equal(entityPair.TotalCount, modelPair.TotalCount);
            }
        }

        /// <summary>
        ///     Validate a <see cref="FoundationRiskDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="FoundationRiskDistribution"/></param>
        /// <param name="model"><see cref="FoundationRiskDistributionResponseModel"/></param>
        internal static void ValidateFoundationRiskDistribution(FoundationRiskDistribution entity, FoundationRiskDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Else, do nullchecking.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            Assert.Equal(entity.PercentageA, model.PercentageA);
            Assert.Equal(entity.PercentageB, model.PercentageB);
            Assert.Equal(entity.PercentageC, model.PercentageC);
            Assert.Equal(entity.PercentageD, model.PercentageD);
            Assert.Equal(entity.PercentageE, model.PercentageE);
        }

        /// <summary>
        ///     Validate a <see cref="FoundationTypeDistributionResponseModel"/>.
        /// </summary>
        /// <param name="entity"><see cref="FoundationTypeDistribution"/></param>
        /// <param name="model"><see cref="FoundationTypeDistributionResponseModel"/></param>
        internal static void ValidateFoundationTypeDistribution(FoundationTypeDistribution entity, FoundationTypeDistributionResponseModel model)
        {
            // If both are null we are good to go.
            if (entity == null && model == null)
            {
                return;
            }

            // Else, do nullchecking.
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            // Assert.
            Assert.Equal(entity.FoundationTypes.Count(), model.FoundationTypes.Count());

            for (int i = 0; i < entity.FoundationTypes.Count(); i++)
            {
                var entityPair = entity.FoundationTypes.ToArray()[i];
                var modelPair = model.FoundationTypes.ToArray()[i];
                Assert.Equal(TestEnumMapper.Map(entityPair.FoundationType), modelPair.FoundationType);
                Assert.Equal(entityPair.TotalCount, modelPair.TotalCount);
            }
        }
    }
}
