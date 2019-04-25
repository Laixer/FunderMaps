using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FunderMaps.Interfaces;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Extensions;
using FunderMaps.Data.Authorization;
using FunderMaps.Models;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Core.Repositories;

namespace FunderMaps.Controllers.Webservice
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

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public SampleController(
            IAuthorizationService authorizationService,
            ISampleRepository sampleRepository,
            IReportRepository reportRepository)
        {
            _authorizationService = authorizationService;
            _sampleRepository = sampleRepository;
            _reportRepository = reportRepository;
        }

        // GET: api/sample
        /// <summary>
        /// Get a chunk of samples either by organization or as public data.
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

            // TODO: attestationOrganizationId can be null
            // TODO: administrator can query anything

            return Ok(await _sampleRepository.ListAllAsync(int.Parse(attestationOrganizationId), new Navigation(offset, limit)));
        }

        // POST: api/sample
        /// <summary>
        /// Create a new sample for a report.
        /// </summary>
        /// <param name="input">Sample data.</param>
        /// <returns>Report.</returns>
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
                GroundwaterLevel = input.GroundwaterLevel,
                GroundLevel = input.GroundLevel,
                FoundationRecoveryAdviced = input.FoundationRecoveryAdviced,
                BuiltYear = input.BuiltYear,
                Address = input.Address,
                FoundationQuality = input.FoundationQuality,
                EnforcementTerm = input.EnforcementTerm,
                Substructure = input.Substructure,
                FoundationType = input.FoundationType,
                BaseMeasurementLevel = BaseLevel.NAP,
                FoundationDamageCause = input.FoundationDamageCause,
                AccessPolicy = AccessPolicy.Private,
            };

            // Set the report status to 'pending'
            report.Status = ReportStatus.Pending;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Create);
            if (authorizationResult.Succeeded)
            {
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
        /// <param name="id">Report identifier.</param>
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

            sample.MonitoringWell = input.MonitoringWell;
            sample.Cpt = input.Cpt;
            sample.WoodLevel = input.WoodLevel;
            sample.GroundLevel = input.GroundLevel;
            sample.GroundwaterLevel = input.GroundwaterLevel;
            sample.BuiltYear = input.BuiltYear;
            sample.Note = input.Note;
            sample.FoundationQuality = input.FoundationQuality;
            sample.EnforcementTerm = input.EnforcementTerm;
            sample.Substructure = input.Substructure;
            sample.FoundationType = input.FoundationType;

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, sample.ReportNavigation.Attribution._Owner, OperationsRequirement.Update);
            if (authorizationResult.Succeeded)
            {
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
