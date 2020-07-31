using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response model representing the distribution of foundation types.
    /// </summary>
    public sealed class FoundationTypeDistributionResponseModel
    {
        /// <summary>
        /// Contains a <see cref="FoundationTypePairResponseModel"/> for each present 
        /// foundation type.
        /// </summary>
        public IEnumerable<FoundationTypePairResponseModel> FoundationTypes { get; set; }
    }
}
