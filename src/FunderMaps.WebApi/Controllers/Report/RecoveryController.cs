using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for recovery operations.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
[Route("api/recovery")]
public sealed class RecoveryController(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    IRecoveryRepository recoveryRepository,
    IBlobStorageService blobStorageService,
    IEmailService emailService) : FunderMapsController
{
    // GET: api/recovery/stats
    /// <summary>
    ///     Return recovery statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await recoveryRepository.CountAsync(),
        };

        return Ok(output);
    }

    // GET: api/recovery/{id}
    /// <summary>
    ///     Return recovery by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public Task<Recovery> GetAsync(int id)
        => recoveryRepository.GetByIdAsync(id, TenantId);

    // GET: api/recovery
    /// <summary>
    ///     Return all recoveries.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<Recovery> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        await foreach (var recovery in recoveryRepository.ListAllAsync(pagination.Navigation, TenantId))
        {
            yield return recovery;
        }
    }

    // POST: api/recovery
    /// <summary>
    ///     Create recovery.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<Recovery> CreateAsync([FromBody] Recovery recovery)
    {
        recovery.Attribution.Creator = UserId;
        recovery.Attribution.Owner = TenantId;

        if (recovery.Attribution.Reviewer == UserId)
        {
            throw new AuthorizationException();
        }

        await recoveryRepository.AddAsync(recovery);

        return await recoveryRepository.GetByIdAsync(recovery.Id, TenantId);
    }

    // POST: api/recovery/upload-document
    /// <summary>
    ///     Upload document to the backstore.
    /// </summary>
    [HttpPost("upload-document")]
    [RequestSizeLimit(128 * 1024 * 1024)]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UploadDocumentAsync([Required][FormFile(Core.Constants.AllowedFileMimes)] IFormFile input)
    {
        string storeFileName = FileHelper.GetUniqueName(input.FileName);
        await blobStorageService.StoreFileAsync(
            containerName: Core.Constants.RecoveryStorageFolderName,
            fileName: storeFileName,
            contentType: input.ContentType,
            stream: input.OpenReadStream());

        var output = new DocumentDto()
        {
            Name = storeFileName,
        };

        return Ok(output);
    }

    // GET: api/recovery/{id}/download
    /// <summary>
    ///     Retrieve document access link.
    /// </summary>
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> GetDocumentAccessLinkAsync(int id)
    {
        var recovery = await recoveryRepository.GetByIdAsync(id, TenantId);
        Uri link = await blobStorageService.GetAccessLinkAsync(
            containerName: Core.Constants.RecoveryStorageFolderName,
            fileName: recovery.DocumentFile,
            hoursValid: 1);

        var result = new BlobAccessLinkDto()
        {
            AccessLink = link
        };

        return Ok(result);
    }

    // PUT: api/recovery/{id}
    /// <summary>
    ///     Update recovery by id.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] Recovery recovery)
    {
        recovery.Id = id;
        recovery.Attribution.Creator = UserId;
        recovery.Attribution.Owner = TenantId;

        if (recovery.Attribution.Reviewer == UserId)
        {
            throw new AuthorizationException();
        }

        await recoveryRepository.UpdateAsync(recovery);

        // FUTURE: Does this make sense?
        // Only when this item was rejected can we move into
        // a pending state after update.
        if (recovery.State.AuditStatus == AuditStatus.Rejected)
        {
            recovery.State.TransitionToPending();
            await recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, TenantId);
        }

        return NoContent();
    }

    // POST: api/recovery/{id}/reset
    /// <summary>
    ///     Reset recovery status to pending by id.
    /// </summary>
    [HttpPost("{id:int}/reset")]
    [Authorize(Policy = "SuperuserAdministratorPolicy")]
    public async Task<IActionResult> ResetAsync(int id)
    {
        var recovery = await recoveryRepository.GetByIdAsync(id, TenantId);

        recovery.State.TransitionToPending();
        await recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, TenantId);

        return NoContent();
    }

    // POST: api/recovery/{id}/status_review
    /// <summary>
    ///     Set recovery status to review by id.
    /// </summary>
    [HttpPost("{id:int}/status_review")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> SetStatusReviewAsync(int id)
    {
        var recovery = await recoveryRepository.GetByIdAsync(id, TenantId);
        var organization = await organizationRepository.GetByIdAsync(TenantId);
        var reviewer = await userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToReview();
        await recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, TenantId);

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = new[] { new EmailAddress(reviewer.Email, reviewer.ToString()) },
            Subject = "FunderMaps - Rapportage ter review",
            Template = "report-reviewer",
            Varaibles = new Dictionary<string, object>
            {
                { "id", recovery.Id },
                { "creatorName", creator.ToString() },
                { "organizationName", organization.Name },
                { "reviewerName", reviewer.ToString() },
                { "documentName", recovery.DocumentName },
            }
        });

        return NoContent();
    }

    // POST: api/recovery/{id}/status_rejected
    /// <summary>
    ///     Set recovery status to rejected by id.
    /// </summary>
    [HttpPost("{id:int}/status_rejected")]
    [Authorize(Policy = "VerifierAdministratorPolicy")]
    public async Task<IActionResult> SetStatusRejectedAsync(int id, StatusChangeDto input)
    {
        var recovery = await recoveryRepository.GetByIdAsync(id, TenantId);
        var reviewer = await userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToRejected();
        await recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, TenantId);

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = new[]
            {
                new EmailAddress(creator.Email, creator.ToString()),
                new EmailAddress(reviewer.Email, reviewer.ToString())
            },
            Subject = "FunderMaps - Rapportage is afgekeurd",
            Template = "report-declined",
            Varaibles = new Dictionary<string, object>
            {
                { "id", recovery.Id },
                { "reviewerName", reviewer.ToString() },
                { "documentName", recovery.DocumentName },
                { "motivation", input.Message },
            }
        });

        return NoContent();
    }

    // POST: api/recovery/{id}/status_approved
    /// <summary>
    ///     Set recovery status to done by id.
    /// </summary>
    [HttpPost("{id:int}/status_approved")]
    [Authorize(Policy = "VerifierAdministratorPolicy")]
    public async Task<IActionResult> SetStatusApprovedAsync(int id)
    {
        var recovery = await recoveryRepository.GetByIdAsync(id, TenantId);
        var reviewer = await userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToDone();
        await recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, TenantId);

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = new[]
            {
                new EmailAddress(creator.Email, creator.ToString()),
                new EmailAddress(reviewer.Email, reviewer.ToString())
            },
            Subject = "FunderMaps - Rapportage is goedgekeurd",
            Template = "report-approved",
            Varaibles = new Dictionary<string, object>
            {
                { "id", recovery.Id },
                { "reviewerName", reviewer.ToString() },
                { "documentName", recovery.DocumentName },
            }
        });

        return NoContent();
    }

    // DELETE: api/recovery/{id}
    /// <summary>
    ///     Delete recovery by id.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SuperuserAdministratorPolicy")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await recoveryRepository.DeleteAsync(id, TenantId);

        return NoContent();
    }
}
