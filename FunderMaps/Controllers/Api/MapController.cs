using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FunderMaps.Data;
using FunderMaps.Data.Authorization;
using FunderMaps.Extensions;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FunderMaps.Controllers.Api
{
    [Authorize]
    [Route("api/map")]
    [ApiController]
    public class MapController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly FisDbContext _dbContext;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public MapController(
            IAuthorizationService authorizationService,
            IConfiguration configuration,
            FisDbContext dbContext)
        {
            _authorizationService = authorizationService;
            _configuration = configuration;
            _dbContext = dbContext;
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
                public int Report { get; set; }
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
        /// Get the samples as GeoJSON.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(FeatureCollection), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // TODO: Administrator can query anything

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            var collection = new List<FeatureModel>();

            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("FISConnection")))
            {
                await connection.OpenAsync();
                var resultSet = await connection.QueryAsync(
                    " SELECT a.*, e.report, st_x(st_transform(a.geopoint, 4326)) as x, st_y(st_transform(a.geopoint, 4326)) as y" +
                    " FROM report.sample AS e" +
                    " JOIN report.address AS a ON e.address = a.id" +
                    " WHERE a.geopoint is NOT NULL" +
                    " ORDER BY e.create_date DESC");

                foreach (var item in resultSet)
                {
                    var sample = item as IDictionary<string, object>;

                    collection.Add(new FeatureModel
                    {
                        Geometry = new FeatureModel.GeometryModel
                        {
                            Coordinates = new double[] { (double)sample["x"], (double)sample["y"] },
                        },
                        Properties = new FeatureModel.PropertyModel()
                        {
                            Name = $"{sample["street_name"]} {sample["building_number"]}",
                            Report = (int)sample["report"],
                        },
                    });
                }
            }

            return Ok(new FeatureCollection
            {
                Features = collection,
            });
        }
    }
}
