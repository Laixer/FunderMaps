using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Map data endpoint.
    /// </summary>
    [Authorize]
    [Route("api/map")]
    [ApiController]
    public class MapController : BaseApiController
    {
        private readonly IMapRepository _mapRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public MapController(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        class FeatureModel
        {
            public class GeometryModel
            {
                public string Type { get; set; } = "Point";
                public double[] Coordinates { get; set; }
            }

            public class PropertyModel
            {
                public string Name { get; set; }
            }

            public string Type { get; set; } = "Feature";
            public GeometryModel Geometry { get; set; }
            public PropertyModel Properties { get; set; }
        }

        class FeatureCollection
        {
            public string Type { get; set; } = "FeatureCollection";
            public ICollection<FeatureModel> Features { get; set; }
        }

        // GET: api/map
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync()
        {
            var points = await _mapRepository.GetByOrganizationIdAsync(User.GetOrganizationId());
            if (points == null)
            {
                return ResourceNotFound();
            }

            var collection = new List<FeatureModel>();
            foreach (var item in points)
            {
                collection.Add(new FeatureModel
                {
                    Geometry = new FeatureModel.GeometryModel
                    {
                        Coordinates = new double[] { item.X, item.Y, item.Z },
                    },
                    Properties = new FeatureModel.PropertyModel()
                    {
                        Name = $"{item.StreetName} {item.BuildingNumber}"
                    },
                });
            }

            return Ok(new FeatureCollection
            {
                Features = collection,
            });
        }
    }
}
