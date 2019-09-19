using FunderMaps.Core.Entities;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Interfaces;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    // FUTURE: Map query interfaces must accept single or multiple items.

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
        public MapController(IMapRepository mapRepository) => _mapRepository = mapRepository;

        class Layer
        {
            /// <summary>
            /// Unique layer identifier.
            /// </summary>
            public Guid Id { get; set; }

            /// <summary>
            /// Visual layer name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Source for layer.
            /// </summary>
            public string Source { get; set; }

            /// <summary>
            /// Layer order.
            /// </summary>
            public int Order { get; set; }
        }

        // GET: api/map/layer
        /// <summary>
        /// Get the samples as GeoJSON.
        /// </summary>
        [HttpGet("layer")]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(IList<Layer>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetLayersAsync()
        {
            var collection = new List<Layer>
            {
                new Layer
                {
                    Id = Guid.NewGuid(),
                    Name = "Analysis",
                    Source = "" // From BAG
                },
                new Layer
                {
                    Id = Guid.NewGuid(),
                    Name = "Alle metingen",
                    Source = "api/map/all"
                },
                new Layer
                {
                    Id = Guid.NewGuid(),
                    Name = "Funderingstype",
                    Source = "api/map/foundation_type"
                },
                new Layer
                {
                    Id = Guid.NewGuid(),
                    Name = "Handhavingstermijnen",
                    Source = "api/map/enforcement_term"
                },
                new Layer
                {
                    Id = Guid.NewGuid(),
                    Name = "Kwaliteit Funderingen",
                    Source = "api/map/foundation_quality"
                }
            };

            await Task.CompletedTask;

            return Ok(collection);
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

        // GET: api/map/all
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("all")]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllSamplesAsync()
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

        private async Task<FeatureCollection> BuildCollection(Task<IReadOnlyList<Core.Entities.AddressPoint>> task)
        {
            var points = await task;
            if (points == null)
            {
                return null;
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

            return new FeatureCollection
            {
                Features = collection,
            };
        }

        // GET: api/map/foundation_type
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("foundation_type")]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetFoundationTypeAsync([FromQuery] int? type)
        {
            // NOTE: This is an temporary solution. The 'type' is nowhere to be defined and therefore bad design.

            switch (type)
            {
                case 0:
                    {
                        var points = await BuildCollection(_mapRepository.GetFounationRecoveryByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                case 1:
                    {
                        var points = await BuildCollection(_mapRepository.GetByFounationTypeWoodByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                case 2:
                    {
                        var points = await BuildCollection(_mapRepository.GetByFounationTypeConcreteByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                case 3:
                    {
                        var points = await BuildCollection(_mapRepository.GetByFounationTypeNoPileByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                case 4:
                    {
                        var points = await BuildCollection(_mapRepository.GetByFounationTypeWoodChargerByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                case 5:
                    {
                        var points = await BuildCollection(_mapRepository.GetByFounationTypeOtherByOrganizationAsync(User.GetOrganizationId()));
                        if (points == null) { break; }
                        return Ok(points);
                    }
                default:
                    break;
            }

            return ResourceNotFound();
        }

        // GET: api/map/enforcement_term
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("enforcement_term")]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetEnforcementTermAsync([FromQuery] int offsetStart, [FromQuery] int offsetEnd)
        {
            var points = await BuildCollection(_mapRepository.GetByEnforcementTermByOrganizationAsync(offsetStart, offsetEnd, User.GetOrganizationId()));
            if (points == null)
            {
                return ResourceNotFound();
            }

            return Ok(points);
        }

        // GET: api/map/foundation_quality
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("foundation_quality")]
        [Authorize(Policy = Constants.OrganizationMemberPolicy)]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetFoundationQualityAsync([FromQuery] FoundationQuality foundationQuality)
        {
            var points = await BuildCollection(_mapRepository.GetByFoundationQualityByOrganizationAsync(foundationQuality, User.GetOrganizationId()));
            if (points == null)
            {
                return ResourceNotFound();
            }

            return Ok(points);
        }
    }
}
