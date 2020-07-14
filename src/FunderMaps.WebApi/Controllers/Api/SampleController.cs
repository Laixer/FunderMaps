namespace FunderMaps.Controllers.Api
{
#if DISABLED
    /// <summary>
    /// Endpoint controller for sample operations.
    /// </summary>
    [Authorize(Policy = Constants.OrganizationMemberPolicy)]
    [Route("api/sample")]
    [ApiController]
    public class SampleController : BaseApiController
    {
        // GET: api/sample
        /// <summary>
        /// Get all samples filtered either by organization or as public data.
        /// </summary>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int offset = 0, [FromQuery] int limit = 25)
            => Ok(await _sampleRepository.ListAllAsync(User.GetOrganizationId(), new Navigation(offset, limit)));

        // GET: api/sample/report/{id}
        /// <summary>
        /// Get all samples filtered by report.
        /// </summary>
        /// <param name="id">Report identifier, see <see cref="Inquiry.Id"/>.</param>
        /// <param name="offset">Offset into the list.</param>
        /// <param name="limit">Limit the output.</param>
        /// <returns>List of samples, see <see cref="Inquiry"/>.</returns>
        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetAllAsync(int id, [FromQuery] int offset = 0, [FromQuery] int limit = 25)
            => Ok(await _sampleRepository.ListAllReportAsync(id, User.GetOrganizationId(), new Navigation(offset, limit)));

        // PUT: api/sample/{id}
        /// <summary>
        /// Update sample if the organization user has access to the record.
        /// </summary>
        /// <param name="id">Sample identifier.</param>
        /// <param name="input">Sample data.</param>
        [HttpPut("{id}")]
        [Authorize(Policy = Constants.OrganizationMemberWritePolicy)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SampleInputOutputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var sample = await _sampleRepository.GetByIdAsync(id, User.GetOrganizationId());
            if (sample == null)
            {
                return ResourceNotFound();
            }

            sample.FoundationType = input.FoundationType;
            sample.MonitoringWell = input.MonitoringWell;
            sample.Cpt = input.Cpt;
            sample.WoodLevel = input.WoodLevel;
            sample.GroundLevel = input.GroundLevel;
            sample.GroundwaterLevel = input.GroundwaterLevel;
            sample.FoundationRecoveryAdviced = input.FoundationRecoveryAdviced;
            sample.FoundationDamageCause = input.FoundationDamageCause;
            sample.BuiltYear = input.BuiltYear;
            sample.FoundationQuality = input.FoundationQuality;
            sample.EnforcementTerm = input.EnforcementTerm;
            sample.Substructure = input.Substructure;
            sample.Note = input.Note;
            sample.Address = await _addressService.GetOrCreateAddressAsync(new Address
            {
                StreetName = input.Address.StreetName,
                BuildingNumber = (short)input.Address.BuildingNumber,
                Bag = input.Address.Bag,
            });

            await _sampleRepository.UpdateAsync(sample);

            return NoContent();
        }
    }
#endif
}
