using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for recovery sample operations.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
[Route("api/recovery/{recoveryId}/sample")]
public class RecoverySampleController(IRecoverySampleRepository recoverySampleRepository, IRecoveryRepository recoveryRepository) : ControllerBase
{
    private readonly IRecoverySampleRepository _recoverySampleRepository = recoverySampleRepository ?? throw new ArgumentNullException(nameof(recoverySampleRepository));
    private readonly IRecoveryRepository _recoveryRepository = recoveryRepository ?? throw new ArgumentNullException(nameof(recoveryRepository));

    // GET: api/recovery/{id}/sample/stats
    /// <summary>
    ///     Return recovery report sample statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync(int recoveryId)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var output = new DatasetStatsDto()
        {
            Count = await _recoverySampleRepository.CountAsync(recoveryId, tenantId),
        };

        return Ok(output);
    }

    // GET: api/recovery/{id}/sample/{id}
    /// <summary>
    ///     Return recovery sample by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<RecoverySample> GetAsync(int id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        return await _recoverySampleRepository.GetByIdAsync(id, tenantId);
    }

    // GET: api/recovery/{id}/sample
    /// <summary>
    ///     Return all recovery samples.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<RecoverySample> GetAllAsync(int recoveryId, [FromQuery] PaginationDto pagination)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await foreach (var recoverySample in _recoverySampleRepository.ListAllAsync(recoveryId, pagination.Navigation, tenantId))
        {
            yield return recoverySample;
        }
    }

    // POST: api/recovery/{id}/sample/{id}
    /// <summary>
    ///     Create recovery sample.
    /// </summary>
    ///         /// <remarks>
    ///     Transition <see cref="Recovery"/> into <see cref="AuditStatus.Pending"/> if a <see cref="RecoverySample"/>
    ///     was successfully created within this <see cref="Recovery"/>.
    /// </remarks>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<RecoverySample> CreateAsync(int recoveryId, [FromBody] RecoverySample recoverySample, [FromServices] IGeocoderTranslation geocoderTranslation)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        // var address = await geocoderTranslation.GetAddressIdAsync(recoverySample.Address);

        // recoverySample.Building = address.BuildingId ?? throw new InvalidOperationException();
        recoverySample.Recovery = recoveryId;

        var recovery = await _recoveryRepository.GetByIdAsync(recoverySample.Recovery, tenantId);
        if (!recovery.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await _recoverySampleRepository.AddAsync(recoverySample);
        recoverySample = await _recoverySampleRepository.GetByIdAsync(recoverySample.Id, tenantId);

        recovery.State.TransitionToPending();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

        return recoverySample;
    }

    // PUT: api/recovery/{id}/sample/{id}
    /// <summary>
    ///     Update recovery sample by id.
    /// </summary>
    /// <remarks>
    ///     Transition <see cref="Recovery"/> into <see cref="AuditStatus.Pending"/> if a <see cref="RecoverySample"/>
    ///     was successfully updated within this <see cref="Recovery"/>.
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UpdateAsync(int recoveryId, int id, [FromBody] RecoverySample recoverySample)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        recoverySample.Id = id;
        recoverySample.Recovery = recoveryId;

        var recovery = await _recoveryRepository.GetByIdAsync(recoverySample.Recovery, tenantId);
        if (!recovery.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await _recoverySampleRepository.UpdateAsync(recoverySample, tenantId);

        recovery.State.TransitionToPending();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

        return NoContent();
    }

    // DELETE: api/recovery/{id}/sample/{id}
    /// <summary>
    ///     Delete recovery sample by id.
    /// </summary>
    /// <remarks>
    ///     Transition <see cref="Recovery"/> into <see cref="AuditStatus.Todo"/> if all <see cref="RecoverySample"/>
    ///     within this <see cref="Recovery"/> are deleted.
    /// </remarks>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recoverySample = await _recoverySampleRepository.GetByIdAsync(id, tenantId);

        var recovery = await _recoveryRepository.GetByIdAsync(recoverySample.Recovery, tenantId);
        if (!recovery.State.AllowWrite)
        {
            throw new EntityReadOnlyException();
        }

        await _recoverySampleRepository.DeleteAsync(recoverySample.Id, tenantId);

        if (await _recoverySampleRepository.CountAsync(recoverySample.Recovery, tenantId) == 0)
        {
            recovery.State.TransitionToTodo();
            await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);
        }

        return NoContent();
    }
}
