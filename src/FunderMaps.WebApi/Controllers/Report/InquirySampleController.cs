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
/// <remarks>
///     Create new instance.
/// </remarks>
[Route("api/inquiry/{inquiryId}/sample")]
public class InquirySampleController(
    IInquiryRepository inquiryRepository,
    IInquirySampleRepository inquirySampleRepository) : ControllerBase
{
    // GET: api/inquiry/{id}/sample/stats
    /// <summary>
    ///     Return inquiry report sample statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync(int inquiryId)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var output = new DatasetStatsDto()
        {
            Count = await inquirySampleRepository.CountAsync(inquiryId, tenantId),
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Return inquiry sample by id.
    /// </summary>
    [HttpGet("{id}")]
    public Task<InquirySample> GetAsync(int id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        return inquirySampleRepository.GetByIdAsync(id, tenantId);
    }

    // GET: api/inquiry/{id}/sample
    /// <summary>
    ///     Return all inquiry samples.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<InquirySample> GetAllAsync(int inquiryId, [FromQuery] PaginationDto pagination)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await foreach (var item in inquirySampleRepository.ListAllAsync(inquiryId, pagination.Navigation, tenantId))
        {
            yield return item;
        }
    }

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

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, tenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        inquirySample.Id = await inquirySampleRepository.AddAsync(inquirySample);

        inquiry.State.TransitionToPending();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);

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
    public async Task<IActionResult> UpdateAsync(int inquiryId, int id, [FromBody] InquirySample inquirySample, [FromServices] IGeocoderTranslation geocoderTranslation)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var address = await geocoderTranslation.GetAddressIdAsync(inquirySample.Address);

        inquirySample.Id = id;
        inquirySample.Inquiry = inquiryId;
        inquirySample.Address = address.Id;
        inquirySample.Building = address.BuildingId ?? throw new InvalidOperationException();

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, tenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await inquirySampleRepository.UpdateAsync(inquirySample, tenantId);

        inquiry.State.TransitionToPending();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);

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

        var inquirySample = await inquirySampleRepository.GetByIdAsync(id, tenantId);

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, tenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await inquirySampleRepository.DeleteAsync(inquirySample.Id, tenantId);

        if (await inquirySampleRepository.CountAsync(inquiry.Id, tenantId) == 0)
        {
            inquiry.State.TransitionToTodo();
            await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, tenantId);
        }

        return NoContent();
    }
}
