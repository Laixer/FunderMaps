using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.IntegrationTests.Services
{
    public class TestDescriptionService : IDescriptionService
    {
        public string GenerateFullDescription(AnalysisProduct product) => "My full test description";
        public string GenerateTerrainDescription(AnalysisProduct product) => "My terrain test description";
    }
}
