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

        // TODO: Add more options such as styling
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
                },
                new Layer
                {
                    Id = Guid.Parse("b47ab81d-14af-4b45-9ba8-2425a56e5f71"),
                    Name = "Pandzakking",
                    Source = "api/map/premise_subsidence",
                    Order = 3
                }
            };

            await Task.CompletedTask;

            return Ok(collection);
        }

        private class FeatureModel
        {
            public string Type { get; set; } = "Feature";
            public object Geometry { get; set; }
            public object Properties { get; set; }
        }

        private class FeatureCollection
        {
            public string Type { get; set; } = "FeatureCollection";
            public ICollection<FeatureModel> Features { get; set; }
        }

        private static FeatureCollection BuildGeoCollection(IEnumerable<AddressGeoJson> points, object properties = null, FeatureCollection featureCollection = null)
        {
            var collection = new List<FeatureModel>();

            if (points != null)
            {
                foreach (var item in points)
                {
                    collection.Add(new FeatureModel
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

            return new FeatureCollection
            {
                Features = collection,
            };
        }

#if _ALL_SAMPLES
        // GET: api/map/all
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSamplesAsync()
        {
            var col1 = await _mapRepository.GetByOrganizationIdAsync(User.GetOrganizationId());

            return Ok(BuildGeoCollection(col1));
        }
#endif

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

            return Ok(BuildGeoCollection(col5, new
            {
                SublayerId = 4,
                Sublayer = "Overig",
                Color = "#FFCD17",
            },
            BuildGeoCollection(col4, new
            {
                SublayerId = 3,
                Sublayer = "Hout met oplanger",
                Color = "#641C68",
            },
            BuildGeoCollection(col3, new
            {
                SublayerId = 2,
                Sublayer = "Niet onderheid",
                Color = "#A30500",
            },
            BuildGeoCollection(col2, new
            {
                SublayerId = 1,
                Sublayer = "Beton paal",
                Color = "#7A7A7A",
            }, BuildGeoCollection(col1, new
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

            return Ok(BuildGeoCollection(col7, new
            {
                SublayerId = 5,
                Sublayer = "Over 20 tot 30 jaar over afgegeven handhavingstermijn",
                Color = "#28922A",
            },
            BuildGeoCollection(col6, new
            {
                SublayerId = 5,
                Sublayer = "Over 10 tot 20 jaar over afgegeven handhavingstermijn",
                Color = "#67B433",
            },
            BuildGeoCollection(col5, new
            {
                SublayerId = 4,
                Sublayer = "Binnen 10 jaar over afgegeven handhavingstermijn",
                Color = "#C7BC3B",
            },
            BuildGeoCollection(col4, new
            {
                SublayerId = 3,
                Sublayer = "Tot 10 jaar over afgegeven handhavingstermijn",
                Color = "#E78932",
            },
            BuildGeoCollection(col3, new
            {
                SublayerId = 2,
                Sublayer = "10 tot 20 jaar over afgegeven handhavingstermijn",
                Color = "#D45925",
            },
            BuildGeoCollection(col2, new
            {
                SublayerId = 1,
                Sublayer = "20 tot 30 jaar over afgegeven handhavingstermijn",
                Color = "#C1301B",
            }, BuildGeoCollection(col1, new
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

            return Ok(BuildGeoCollection(col6, new
            {
                SublayerId = 5,
                Sublayer = "Goede staat",
                Color = "#28922A",
            },
            BuildGeoCollection(col5, new
            {
                SublayerId = 4,
                Sublayer = "Redelijke staat",
                Color = "#67B433",
            },
            BuildGeoCollection(col4, new
            {
                SublayerId = 3,
                Sublayer = "Acceptabele staat",
                Color = "#C7BC3B",
            },
            BuildGeoCollection(col3, new
            {
                SublayerId = 2,
                Sublayer = "Twijfelachtige staat",
                Color = "#E78932",
            },
            BuildGeoCollection(col2, new
            {
                SublayerId = 1,
                Sublayer = "Slechte staat",
                Color = "#C1301B",
            }, BuildGeoCollection(col1, new
            {
                SublayerId = 0,
                Sublayer = "Zeer slechte staat",
                Color = "#B61F17",
            })))))));
        }

        // GET: api/map/premise_subsidence
        /// <summary>
        /// Get the samples as GeoJson.
        /// </summary>
        [HttpGet("premise_subsidence")]
        public async Task<IActionResult> GetPremiseSubsidenceAsync()
        {
            // FUTURE: This is super inefficient. We can query everything together and build a local collection.

            var col1 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(9, 100, User.GetOrganizationId());
            var col2 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(8, 9, User.GetOrganizationId());
            var col3 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(7, 8, User.GetOrganizationId());
            var col4 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(6, 7, User.GetOrganizationId());
            var col5 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(5, 6, User.GetOrganizationId());
            var col6 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(4, 5, User.GetOrganizationId());
            var col7 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(3, 4, User.GetOrganizationId());
            var col8 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(2, 3, User.GetOrganizationId());
            var col9 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(1, 2, User.GetOrganizationId());
            var col10 = await _mapRepository.GetByFoundationSubsidenceByOrganizationAsync(0, 1, User.GetOrganizationId());

            return Ok(BuildGeoCollection(col10, new
            {
                SublayerId = 5,
                Sublayer = "Minder dan 1mm/jaar of rijzing",
                Color = "#293575",
            },
            BuildGeoCollection(col9, new
            {
                SublayerId = 5,
                Sublayer = "1 tot 2 mm/jaar",
                Color = "#1261A3",
            },
            BuildGeoCollection(col8, new
            {
                SublayerId = 4,
                Sublayer = "2 tot 3 mm/jaar",
                Color = "#69A8DE",
            },
            BuildGeoCollection(col7, new
            {
                SublayerId = 4,
                Sublayer = "3 tot 4 mm/jaar",
                Color = "#99C1E9",
            },
            BuildGeoCollection(col6, new
            {
                SublayerId = 3,
                Sublayer = "4 tot 5 mm/jaar",
                Color = "#B378B1",
            },
            BuildGeoCollection(col5, new
            {
                SublayerId = 2,
                Sublayer = "5 tot 6 mm/jaar",
                Color = "#A860A6",
            },
            BuildGeoCollection(col4, new
            {
                SublayerId = 1,
                Sublayer = "6 tot 7 mm/jaar",
                Color = "#8F3C8D",
            },
            BuildGeoCollection(col3, new
            {
                SublayerId = 0,
                Sublayer = "7 tot 8 mm/jaar",
                Color = "#641C68",
            },
            BuildGeoCollection(col2, new
            {
                SublayerId = 0,
                Sublayer = "8 tot 9 mm/jaar",
                Color = "#E15601",
            },
            BuildGeoCollection(col1, new
            {
                SublayerId = 0,
                Sublayer = "9 mm/jaar of meer",
                Color = "#A30500",
            })))))))))));
        }

#if _TILESET
        // GET: api/map/foundation_quality2
        /// <summary>
        /// Get the samples as Tileset.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("vector/{z}/{x}/{y}/foundation_quality2.pbf")]
        public async Task<IActionResult> GetFoundationQuality2Async(int z, int x, int y)
        {
            static double[] TileToGPSPos(double tile_x, double tile_y, int zoom)
            {
                double n = Math.PI - ((2.0 * Math.PI * tile_y) / Math.Pow(2.0, zoom));

                var X = (float)((tile_x / Math.Pow(2.0, zoom) * 360.0) - 180.0);
                var Y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

                return new double[] { X, Y };
            }

            var min = TileToGPSPos(x, y, z);
            var max = TileToGPSPos(x + 1, y + 1, z);

            var fileRef = _blobClient.GetContainerReference("tileset").GetBlockBlobReference($"{z}.{x}.{y}.pbf");
            if (await fileRef.ExistsAsync())
            {
                var memstream = new System.IO.MemoryStream();
                await fileRef.DownloadToStreamAsync(memstream).ConfigureAwait(false);
                memstream.Seek(0, System.IO.SeekOrigin.Begin);
                return new FileStreamResult(memstream, "application/x-protobuf");
            }

            using var cnn = _dbProvider.ConnectionScope();
            var sql = $@"
                SELECT ST_AsMVT(tile)
                FROM (
                    SELECT addr.street_name,
                      addr.building_number,
                      samp.report,
                      st_asmvtgeom(prem.geom,
                                         ST_MakeEnvelope({min[0]}, {min[1]}, {max[0]}, {max[1]}, 4326),
                          4096, 256, false) AS geom
                    FROM   application.sample AS samp
                      INNER JOIN application.report AS reprt ON samp.report = reprt.id
                      INNER JOIN application.attribution AS attr ON reprt.attribution = attr.id
                      INNER JOIN application.address AS addr ON samp.address = addr.id
                      INNER JOIN geospatial.residential_object AS reso ON addr.bag = reso.designation
                      INNER JOIN geospatial.premise AS prem ON reso.id = prem.residential_object
                    WHERE   addr.bag IS NOT NULL
                    AND ST_Contains (
                      ST_MakeEnvelope({min[0]}, {min[1]}, {max[0]}, {max[1]}, 4326),
                      prem.geom)
                ) AS tile";

            var rs = cnn.Query<byte[]>(sql).FirstOrDefault();

            fileRef.Properties.ContentType = "application/x-protobuf";
            await fileRef.UploadFromByteArrayAsync(rs, 0, rs.Length);

            return File(rs, "application/x-protobuf");
        }
#endif
    }
}
