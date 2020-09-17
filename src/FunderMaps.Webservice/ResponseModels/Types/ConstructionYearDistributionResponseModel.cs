using System.Collections.Generic;
using System.Linq;

namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response model representing the distribution of building construction years.
    /// </summary>
    public sealed class ConstructionYearDistributionResponseModel
    {
        /// <summary>
        /// Represents each decade in which the construction year of one or 
        /// more buildings exist, including the total amount of buildings per
        /// decade.
        /// </summary>
        public IEnumerable<ConstructionYearPairResponseModel> Decades { get; set; }

        /// <summary>
        /// Represents the total amount of buildings in all <see cref="Decades"/>.
        /// </summary>
        public uint TotalBuildings => (uint)((Decades != null) ? Decades.Sum(x => x.TotalCount) : 0);
    }
}
