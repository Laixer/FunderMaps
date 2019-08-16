using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FunderMaps.Interfaces;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Extensions;
using FunderMaps.Data.Authorization;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;
using FunderMaps.ViewModels;
using FunderMaps.Helpers;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Endpoint controller for sample operations.
    /// </summary>
    [Authorize]
    [Route("api/sample")]
    [ApiController]
    public class SampleController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ISampleRepository _sampleRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IAddressRepository _addressRepository;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public SampleController(
            IAuthorizationService authorizationService,
            ISampleRepository sampleRepository,
            IReportRepository reportRepository,
            IAddressRepository addressRepository)
        {
            _authorizationService = authorizationService;
            _sampleRepository = sampleRepository;
            _reportRepository = reportRepository;
            _addressRepository = addressRepository;
        }

        // GET: api/sample
        /// <summary>
        /// Get all samples filtered either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Sample>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _sampleRepository.ListAllAsync(new Navigation(offset, limit)));
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            return Ok(await _sampleRepository.ListAllAsync(int.Parse(attestationOrganizationId), new Navigation(offset, limit)));
        }

        // GET: api/sample/report/{id}
        /// <summary>
        /// Get all samples filtered by report.
        /// </summary>
        /// <param name="id">Report identifier, see <see cref="Report.Id"/>.</param>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples, see <see cref="Report"/>.</returns>
        [HttpGet("report/{id}")]
        [ProducesResponseType(typeof(List<Sample>), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAllAsync(int id, [FromQuery] int offset = 0, [FromQuery] int limit = 25)
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                return Ok(await _sampleRepository.ListAllReportAsync(id, new Navigation(offset, limit)));
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            return Ok(await _sampleRepository.ListAllReportAsync(id, int.Parse(attestationOrganizationId), new Navigation(offset, limit)));
        }

        // GET: api/sample/stats
        /// <summary>
        /// Return entity statistics.
        /// </summary>
        /// <returns>EntityStatsOutputModel.</returns>
        [HttpGet("stats")]
        [ProducesResponseType(typeof(EntityStatsOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetStatsAsync()
        {
            var attestationOrganizationId = User.GetClaim(FisClaimTypes.OrganizationAttestationIdentifier);

            // Administrator can query anything
            if (User.IsInRole(Constants.AdministratorRole))
            {
                // TODO: Move into repo
                var _sql = @"SELECT COUNT(*)
                            FROM   report.sample AS samp
                                    INNER JOIN report.address AS addr ON samp.address = addr.id
                                    INNER JOIN report.report AS reprt ON samp.report = reprt.id
                                    INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                            WHERE  samp.delete_date IS NULL";

                using (var connection = _dbProvider.ConnectionScope())
                {
                    return Ok(new EntityStatsOutputModel
                    {
                        Count = await connection.QuerySingleAsync<int>(_sql)
                    });
                }
            }

            if (attestationOrganizationId == null)
            {
                return ResourceForbid();
            }

            // TODO: Move into repo
            var sql = @"SELECT COUNT(*)
                        FROM   report.sample AS samp
                                INNER JOIN report.address AS addr ON samp.address = addr.id
                                INNER JOIN report.report AS reprt ON samp.report = reprt.id
                                INNER JOIN report.attribution AS attr ON reprt.attribution = attr.id
                        WHERE  samp.delete_date IS NULL
                                AND (attr.owner = @Owner
                                        OR samp.access_policy = 'public')";

            using (var connection = _dbProvider.ConnectionScope())
            {
                return Ok(new EntityStatsOutputModel
                {
                    Count = await connection.QuerySingleAsync<int>(sql, new { Owner = int.Parse(attestationOrganizationId) })
                });
            }
        }

        // POST: api/sample
        /// <summary>
        /// Create a new sample for a report.
        /// </summary>
        /// <param name="input">See <see cref="Sample"/>.</param>
        /// <returns>See <see cref="Sample"/>.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PostAsync([FromBody] Sample input)
        {
            var report = await _reportRepository.GetByIdAsync(input.Report.Value);
            if (report == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, report.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
                // Check if we can add new samples to the report
                if (report.Status != ReportStatus.Todo && report.Status != ReportStatus.Pending)
                {
                    return Forbid(0, "Resource modification forbidden with current status");
                }

                var sample = new Sample
                {
                    ReportNavigation = report,
                    MonitoringWell = input.MonitoringWell,
                    Cpt = input.Cpt,
                    Note = input.Note,
                    WoodLevel = input.WoodLevel,
                    GroundLevel = input.GroundLevel,
                    GroundwaterLevel = input.GroundwaterLevel,
                    FoundationRecoveryAdviced = input.FoundationRecoveryAdviced,
                    BuiltYear = input.BuiltYear,
                    FoundationQuality = input.FoundationQuality,
                    EnforcementTerm = input.EnforcementTerm,
                    Substructure = input.Substructure,
                    FoundationType = input.FoundationType,
                    BaseMeasurementLevel = BaseLevel.NAP,
                    FoundationDamageCause = input.FoundationDamageCause,
                    AccessPolicy = input.AccessPolicy,
                    Address = await _addressRepository.GetOrAddAsync(input.Address),
                };

                // Set the report status to 'pending'
                report.Status = ReportStatus.Pending;

                await _sampleRepository.AddAsync(sample);
                await _reportRepository.UpdateAsync(report);

                return Ok(sample);
            }

            return ResourceForbid();
        }

        // GET: api/sample/{id}
        /// <summary>
        /// Retrieve the sample by identifier. The sample is returned
        /// if the the record is public or if the organization user has
        /// access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <returns>Report.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Sample), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            // Public data is accessible to anyone
            if (sample.IsPublic() && sample.ReportNavigation.IsPublic())
            {
                return Ok(sample);
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Read);
            if (authorizationResult.Succeeded)
            {
                return Ok(sample);
            }

            return ResourceForbid();
        }

        // PUT: api/sample/{id}
        /// <summary>
        /// Update sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="input">Sample data.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Sample input)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            if (id != input.Id)
            {
                return BadRequest(0, "Identifiers do not match entity");
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
                sample.MonitoringWell = input.MonitoringWell;
                sample.Cpt = input.Cpt;
                sample.Note = input.Note;
                sample.WoodLevel = input.WoodLevel;
                sample.GroundLevel = input.GroundLevel;
                sample.GroundwaterLevel = input.GroundwaterLevel;
                sample.FoundationRecoveryAdviced = input.FoundationRecoveryAdviced;
                sample.BuiltYear = input.BuiltYear;
                sample.FoundationQuality = input.FoundationQuality;
                sample.EnforcementTerm = input.EnforcementTerm;
                sample.Substructure = input.Substructure;
                sample.FoundationType = input.FoundationType;
                sample.FoundationDamageCause = input.FoundationDamageCause;
                sample.AccessPolicy = input.AccessPolicy;
                sample.Address = await _addressRepository.GetOrAddAsync(input.Address);

                await _sampleRepository.UpdateAsync(sample);

                return NoContent();
            }

            return ResourceForbid();
        }

        // DELETE: api/sample/{id}
        /// <summary>
        /// Soft delete the sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var sample = await _sampleRepository.GetByIdAsync(id);
            if (sample == null)
            {
                return ResourceNotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Delete);
            if (authorizationResult.Succeeded)
            {
                await _sampleRepository.DeleteAsync(sample);

                return NoContent();
            }

            return ResourceForbid();
        }
    }
}
