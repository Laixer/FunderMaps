using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Webservice.ResponseModels.Types
{
    /// <summary>
    /// Response model representing a year.
    /// </summary>
    public sealed class YearResponseModel
    {
        /// <summary>
        /// The year this represents.
        /// </summary>
        public int YearValue { get; set; }
    }
}
