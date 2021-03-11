using System;
using System.Collections.Generic;
using System.Linq;
using FunderMaps.Core.Types;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     Date range.
    /// </summary>
    public record YearsResponseModel
    {
        /// <summary>
        ///     The first year for this collection of years.
        /// </summary>
        public DateTimeOffset YearFrom { get; init; }

        /// <summary>
        ///     The last year in this collection of years.
        /// </summary>
        public DateTimeOffset YearTo { get; init; }
    }

    /// <summary>
    ///     Response model representing the distribution of building construction years.
    /// </summary>
    public record ConstructionYearDistributionResponseModel
    {
        /// <summary>
        ///     Represents each decade in which the construction year of one or 
        ///     more buildings exist, including the total amount of buildings per
        /// decade.
        /// </summary>
        public IEnumerable<ConstructionYearPairResponseModel> Decades { get; init; }

        /// <summary>
        ///     Represents the total amount of buildings in all <see cref="Decades"/>.
        /// </summary>
        public long TotalBuildings => Decades?.Sum(x => x.TotalCount) ?? 0;
    }

    /// <summary>
    ///     Response model representing how many buildings have their construction 
    ///     years in a given <see cref="Decade"/>.
    /// </summary>
    public record ConstructionYearPairResponseModel
    {
        /// <summary>
        ///     Decade that represents this construction year pair.
        /// </summary>
        public YearsResponseModel Decade { get; init; }

        /// <summary>
        ///     Total amount of items that fall into this decade.
        /// </summary>
        public uint TotalCount { get; init; }
    }

    /// <summary>
    ///     Response model containing the distribution of foundation risks.
    /// </summary>
    public record FoundationRiskDistributionResponseModel
    {
        /// <summary>
        ///     Percentage of foundations having risk A.
        /// </summary>
        public double PercentageA { get; init; }

        /// <summary>
        ///     Percentage of foundations having risk B.
        /// </summary>
        public double PercentageB { get; init; }

        /// <summary>
        ///     Percentage of foundations having risk C.
        /// </summary>
        public double PercentageC { get; init; }

        /// <summary>
        ///     Percentage of foundations having risk D.
        /// </summary>
        public double PercentageD { get; init; }

        /// <summary>
        /// Percentage of foundations having risk E.
        /// </summary>
        public double PercentageE { get; init; }
    }

    /// <summary>
    ///     Response model representing the distribution of foundation types.
    /// </summary>
    public record FoundationTypeDistributionResponseModel
    {
        /// <summary>
        ///     Contains a <see cref="FoundationTypePairResponseModel"/> for each present 
        ///     foundation type.
        /// </summary>
        public IEnumerable<FoundationTypePairResponseModel> FoundationTypes { get; init; }
    }

    /// <summary>
    ///     Response model containing the amount of buildings which have a given
    /// <see cref="FoundationType"/>.
    /// </summary>
    public record FoundationTypePairResponseModel
    {
        /// <summary>
        ///     The type of foundation.
        /// </summary>
        public FoundationType FoundationType { get; init; }

        /// <summary>
        ///     The percentage of buildings having this foundation type.
        /// </summary>
        public double Percentage { get; init; }
    }

    /// <summary>
    ///     Response model representing how incidents per year.
    /// </summary>
    public record IncidentYearPairResponseModel
    {
        /// <summary>
        ///     Per year statistics.
        /// </summary>
        public int Year { get; init; }

        /// <summary>
        ///     Total amount of items that fall into this decade.
        /// </summary>
        public uint TotalCount { get; init; }
    }

    /// <summary>
    ///     Response model representing how inquiries per year.
    /// </summary>
    public record InquiryYearPairResponseModel
    {
        /// <summary>
        ///     Per year statistics.
        /// </summary>
        public int Year { get; init; }

        /// <summary>
        ///     Total amount of items that fall into this decade.
        /// </summary>
        public uint TotalCount { get; init; }
    }

    /// <summary>
    ///     Statistics DTO.
    /// </summary>
    public record StatisticsDto
    {
        /// <summary>
        ///     Represents the distribution of foundation types.
        /// </summary>
        public FoundationTypeDistributionResponseModel FoundationTypeDistribution { get; init; }

        /// <summary>
        ///     Represents the distribution of building construction years in the
        ///     given region.
        /// </summary>
        public ConstructionYearDistributionResponseModel ConstructionYearDistribution { get; init; }

        /// <summary>
        ///     Represents the distribution of foundation risks in the given region.
        /// </summary>
        public FoundationRiskDistributionResponseModel FoundationRiskDistribution { get; init; }

        /// <summary>
        ///     Represents the percentage of collected data in the given region.
        /// </summary>
        public double? DataCollectedPercentage { get; init; }

        /// <summary>
        ///     Total amount of restored buildings in the given area.
        /// </summary>
        public int? TotalBuildingRestoredCount { get; init; }

        /// <summary>
        ///     Total amount of incidents in the given region.
        /// </summary>
        public IEnumerable<IncidentYearPairResponseModel> TotalIncidentCount { get; init; }

        /// <summary>
        ///     Total amount of incidents in the given region.
        /// </summary>
        public IEnumerable<IncidentYearPairResponseModel> MunicipalityIncidentCount { get; init; }

        /// <summary>
        ///     Total amount of reports in the given region.
        /// </summary>
        public IEnumerable<InquiryYearPairResponseModel> TotalReportCount { get; init; }

        /// <summary>
        ///     Total amount of reports in the given region.
        /// </summary>
        public IEnumerable<InquiryYearPairResponseModel> MunicipalityReportCount { get; init; }
    }
}
