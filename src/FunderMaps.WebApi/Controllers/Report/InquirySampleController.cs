using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for inquiry sample operations.
/// </summary>
[Route("api/inquiry/{inquiryId}/sample")]
public class InquirySampleController : ControllerBase
{
    private readonly IInquiryRepository _inquiryRepository;
    private readonly IInquirySampleRepository _inquirySampleRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public InquirySampleController(IInquiryRepository inquiryRepository, IInquirySampleRepository inquirySampleRepository)
    {
        _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
        _inquirySampleRepository = inquirySampleRepository ?? throw new ArgumentNullException(nameof(inquirySampleRepository));
    }

    // GET: api/inquiry/{id}/sample/stats
    /// <summary>
    ///     Return inquiry report sample statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync(int inquiryId)
    {
        var output = new DatasetStatsDto()
        {
            Count = await _inquirySampleRepository.CountAsync(inquiryId),
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Return inquiry sample by id.
    /// </summary>
    [HttpGet("{id}")]
    public Task<InquirySample> GetAsync(int id)
        => _inquirySampleRepository.GetByIdAsync(id);

    // GET: api/inquiry/{id}/sample
    /// <summary>
    ///     Return all inquiry samples.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<InquirySample> GetAllAsync(int inquiryId, [FromQuery] PaginationDto pagination)
        => _inquirySampleRepository.ListAllAsync(inquiryId, pagination.Navigation);

    // POST: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Create inquiry sample.
    /// </summary>
    /// <remarks>
    ///     Transition <see cref="Inquiry"/> into state pending if a <see cref="InquirySample"/>
    ///     was successfully created within this <see cref="Inquiry"/>.
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<InquirySample> CreateAsync(int inquiryId, [FromBody] InquirySample inquirySample, [FromServices] IGeocoderTranslation geocoderTranslation)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var address = await geocoderTranslation.GetAddressIdAsync(inquirySample.Address);

        inquirySample.Address = address.Id;
        inquirySample.Building = address.BuildingId ?? throw new InvalidOperationException();
        inquirySample.Inquiry = inquiryId;

        var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        inquirySample = await _inquirySampleRepository.AddGetAsync(inquirySample);

        inquiry.State.TransitionToPending();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);

        return inquirySample;
    }

    // PUT: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Update inquiry sample by id.
    /// </summary>
    /// <remarks>
    ///     Transition <see cref="Inquiry"/> into state pending if a <see cref="InquirySample"/>
    ///     was successfully updated within this <see cref="Inquiry"/>.
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UpdateAsync(int inquiryId, int id, [FromBody] InquirySample inquirySample)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        inquirySample.Id = id;
        inquirySample.Inquiry = inquiryId;

        var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await _inquirySampleRepository.UpdateAsync(inquirySample);

        inquiry.State.TransitionToPending();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);

        return NoContent();
    }

    // DELETE: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Delete inquiry sample by id.
    /// </summary>
    /// <remarks>
    ///     Transition <see cref="Inquiry"/> into state todo if all <see cref="InquirySample"/>
    ///     within this <see cref="Inquiry"/> are deleted.
    /// </remarks>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var inquirySample = await _inquirySampleRepository.GetByIdAsync(id);

        var inquiry = await _inquiryRepository.GetByIdAsync(inquirySample.Inquiry);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await _inquirySampleRepository.DeleteAsync(inquirySample.Id);

        // FUTURE: Should only select inquiry
        if (await _inquirySampleRepository.CountAsync() == 0)
        {
            inquiry.State.TransitionToTodo();
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);
        }

        return NoContent();
    }
}
