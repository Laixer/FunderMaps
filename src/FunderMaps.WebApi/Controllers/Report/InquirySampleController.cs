using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
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
public sealed class InquirySampleController(
    IInquiryRepository inquiryRepository,
    IInquirySampleRepository inquirySampleRepository,
    GeocoderTranslation geocoderTranslation) : FunderMapsController
{
    // GET: api/inquiry/{id}/sample/stats
    /// <summary>
    ///     Return inquiry report sample statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync(int inquiryId)
    {
        var output = new DatasetStatsDto()
        {
            Count = await inquirySampleRepository.CountAsync(inquiryId, TenantId),
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}/sample/{id}
    /// <summary>
    ///     Return inquiry sample by id.
    /// </summary>
    [HttpGet("{id}")]
    public Task<InquirySample> GetAsync(int id)
        => inquirySampleRepository.GetByIdAsync(id, TenantId);

    // GET: api/inquiry/{id}/sample
    /// <summary>
    ///     Return all inquiry samples.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<InquirySample> GetAllAsync(int inquiryId, [FromQuery] PaginationDto pagination)
    {
        await foreach (var item in inquirySampleRepository.ListAllAsync(inquiryId, pagination.Navigation, TenantId))
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
    public async Task<InquirySample> CreateAsync(int inquiryId, [FromBody] InquirySample inquirySample)
    {
        var address = await geocoderTranslation.GetAddressIdAsync(inquirySample.Address);

        inquirySample.Address = address.Id;
        inquirySample.Building = address.BuildingId ?? throw new InvalidOperationException();
        inquirySample.Inquiry = inquiryId;

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, TenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        inquirySample.Id = await inquirySampleRepository.AddAsync(inquirySample);

        inquiry.State.TransitionToPending();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

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
        var address = await geocoderTranslation.GetAddressIdAsync(inquirySample.Address);

        inquirySample.Id = id;
        inquirySample.Inquiry = inquiryId;
        inquirySample.Address = address.Id;
        inquirySample.Building = address.BuildingId ?? throw new InvalidOperationException();

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, TenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await inquirySampleRepository.UpdateAsync(inquirySample, TenantId);

        inquiry.State.TransitionToPending();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

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
        var inquirySample = await inquirySampleRepository.GetByIdAsync(id, TenantId);

        var inquiry = await inquiryRepository.GetByIdAsync(inquirySample.Inquiry, TenantId);
        if (!inquiry.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await inquirySampleRepository.DeleteAsync(inquirySample.Id, TenantId);

        if (await inquirySampleRepository.CountAsync(inquiry.Id, TenantId) == 0)
        {
            inquiry.State.TransitionToTodo();
            await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);
        }

        return NoContent();
    }
}
