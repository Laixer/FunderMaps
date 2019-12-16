using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    // FUTURE: Map query interfaces must accept single or multiple items.

    /// <summary>
    /// Map data endpoint.
    /// </summary>
    [Authorize(Policy = Constants.OrganizationMemberPolicy)]
    [Route("api/map")]
    [ApiController]
    public class MapController : BaseApiController
    {
        private readonly IMapRepository _mapRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="mapRepository">See <see cref="IMapRepository"/>.</param>
        public MapController(IMapRepository mapRepository) => _mapRepository = mapRepository;

        private class Layer
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
        [ResponseCache(Duration = 60 * 60 * 2, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetLayersAsync()
        {
            var collection = new List<Layer>
            {
                //new Layer
                //{
                //    Id = Guid.Parse("24318b03-e7af-4519-a4e1-d12c485e1493"),
                //    Name = "Analysis",
                //    Source = "", // From BAG
                //},
                //new Layer
                //{
                //    Id = Guid.Parse("e696373c-d1a7-4f1b-ab7d-853d1e5d370a"),
                //    Name = "Alle metingen",
                //    Source = "api/map/all"
                //},
                new Layer
                {
                    Id = Guid.Parse("9319cd4f-9387-4401-8641-0e982780c133"),
                    Name = "Funderingstype",
                    Source = "api/map/foundation_type",
                    Order = 0
                },
                new Layer
                {
                    Id = Guid.Parse("8a8fbafc-6975-487b-824d-332ecd2c44af"),
                    Name = "Handhavingstermijnen",
                    Source = "api/map/enforcement_term",
                    Order = 1
                },
                new Layer
                {
                    Id = Guid.Parse("eca38907-95f0-46c7-9226-e0abe1ff3e91"),
                    Name = "Kwaliteit Funderingen",
                    Source = "api/map/foundation_quality",
                    Order = 2
                }
            };

            await Task.CompletedTask;

            return Ok(collection);
        }

        private class FeatureModel2
        {
            public string Type { get; set; } = "Feature";
            public object Geometry { get; set; }
            public object Properties { get; set; }
        }

        private class FeatureCollection2
        {
            public string Type { get; set; } = "FeatureCollection";
            public ICollection<FeatureModel2> Features { get; set; }
        }

        private static FeatureCollection2 BuildGeoCollection(IEnumerable<AddressGeoJson> points, object properties = null, FeatureCollection2 featureCollection = null)
        {
            var collection = new List<FeatureModel2>();

            if (points != null)
            {
                foreach (var item in points)
                {
                    collection.Add(new FeatureModel2
                    {
                        Geometry = JsonConvert.DeserializeObject(item.GeoJson),
                        Properties = properties,
                    });
                }
            }

            if (featureCollection != null)
            {
                collection.AddRange(featureCollection.Features);
            }

            return new FeatureCollection2
            {
                Features = collection,
            };
        }

        // TODO: Maybe remove?
        // GET: api/map/all
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllSamplesAsync()
        //{
        //    var col1 = await _mapRepository.GetByOrganizationIdAsync(User.GetOrganizationId());

        //    return Ok(BuildGeoCollection(col1));
        //}

        // GET: api/map/foundation_type
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("foundation_type")]
        public async Task<IActionResult> GetFoundationTypeAsync()
        {
            // FUTURE: This is super inefficient. We can query everything together and build a local collection.

            var col1 = await _mapRepository.GetByFounationTypeWoodByOrganizationAsync(User.GetOrganizationId());
            var col2 = await _mapRepository.GetByFounationTypeConcreteByOrganizationAsync(User.GetOrganizationId());
            var col3 = await _mapRepository.GetByFounationTypeNoPileByOrganizationAsync(User.GetOrganizationId());
            var col4 = await _mapRepository.GetByFounationTypeWoodChargerByOrganizationAsync(User.GetOrganizationId());
            var col5 = await _mapRepository.GetByFounationTypeOtherByOrganizationAsync(User.GetOrganizationId());

            return Ok(BuildGeoCollection2(col5, new
            {
                SublayerId = 4,
                Sublayer = "Overig",
                Color = "#E78932",
            },
            BuildGeoCollection2(col4, new
            {
                SublayerId = 3,
                Sublayer = "Hout met oplanger",
                Color = "#D36E2C",
            },
            BuildGeoCollection2(col3, new
            {
                SublayerId = 2,
                Sublayer = "Niet onderheid",
                Color = "#C1301B",
            },
            BuildGeoCollection2(col2, new
            {
                SublayerId = 1,
                Sublayer = "Beton paal",
                Color = "#9F9E9E",
            }, BuildGeoCollection2(col1, new
            {
                SublayerId = 0,
                Sublayer = "Houten paal",
                Color = "#9E511F",
            }))))));
        }

        // GET: api/map/enforcement_term
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("enforcement_term")]
        public async Task<IActionResult> GetEnforcementTermAsync()
        {
            // FUTURE: This is super inefficient. We can query everything together and build a local collection.

            var col1 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(-100, -30, User.GetOrganizationId());
            var col2 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(-30, -20, User.GetOrganizationId());
            var col3 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(-20, -10, User.GetOrganizationId());
            var col4 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(-10, 0, User.GetOrganizationId());
            var col5 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(0, 10, User.GetOrganizationId());
            var col6 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(10, 20, User.GetOrganizationId());
            var col7 = await _mapRepository.GetByEnforcementTermByOrganizationAsync(20, 30, User.GetOrganizationId());

            return Ok(BuildGeoCollection2(col7, new
            {
                SublayerId = 5,
                Sublayer = "Over 20 tot 30 jaar over afgegeven handhavingstermijn",
                Color = "#28922A",
            },
            BuildGeoCollection2(col6, new
            {
                SublayerId = 5,
                Sublayer = "Over 10 tot 20 jaar over afgegeven handhavingstermijn",
                Color = "#67B433",
            },
            BuildGeoCollection2(col5, new
            {
                SublayerId = 4,
                Sublayer = "Binnen 10 jaar over afgegeven handhavingstermijn",
                Color = "#C7BC3B",
            },
            BuildGeoCollection2(col4, new
            {
                SublayerId = 3,
                Sublayer = "Tot 10 jaar over afgegeven handhavingstermijn",
                Color = "#E78932",
            },
            BuildGeoCollection2(col3, new
            {
                SublayerId = 2,
                Sublayer = "10 tot 20 jaar over afgegeven handhavingstermijn",
                Color = "#D45925",
            },
            BuildGeoCollection2(col2, new
            {
                SublayerId = 1,
                Sublayer = "20 tot 30 jaar over afgegeven handhavingstermijn",
                Color = "#C1301B",
            }, BuildGeoCollection2(col1, new
            {
                SublayerId = 0,
                Sublayer = "Meer dan 30 jaar over afgegeven handhavingstermijn",
                Color = "#B61F17",
            }))))))));
        }

        // GET: api/map/foundation_quality
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("foundation_quality")]
        public async Task<IActionResult> GetFoundationQualityAsync()
        {
            // FUTURE: This is super inefficient. We can query everything together and build a local collection.

            var col1 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.Bad, User.GetOrganizationId());
            var col2 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.MediocreBad, User.GetOrganizationId());
            var col3 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.Mediocre, User.GetOrganizationId());
            var col4 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.Tolerable, User.GetOrganizationId());
            var col5 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.MediocreGood, User.GetOrganizationId());
            var col6 = await _mapRepository.GetByFoundationQualityByOrganizationAsync(FoundationQuality.Good, User.GetOrganizationId());

            return Ok(BuildGeoCollection2(col6, new
            {
                SublayerId = 5,
                Sublayer = "Goede staat",
                Color = "#28922A",
            },
            BuildGeoCollection2(col5, new
            {
                SublayerId = 4,
                Sublayer = "Redelijke staat",
                Color = "#67B433",
            },
            BuildGeoCollection2(col4, new
            {
                SublayerId = 3,
                Sublayer = "Acceptabele staat",
                Color = "#C7BC3B",
            },
            BuildGeoCollection2(col3, new
            {
                SublayerId = 2,
                Sublayer = "Twijfelachtige staat",
                Color = "#E78932",
            },
            BuildGeoCollection2(col2, new
            {
                SublayerId = 1,
                Sublayer = "Slechte staat",
                Color = "#C1301B",
            }, BuildGeoCollection2(col1, new
            {
                SublayerId = 0,
                Sublayer = "Zeer slechte staat",
                Color = "#B61F17",
            })))))));
        }
    }
}
