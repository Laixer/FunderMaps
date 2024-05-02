using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for inquiry operations.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
[Route("api/inquiry")]
public sealed class InquiryController(
    IOrganizationRepository organizationRepository,
    IUserRepository userRepository,
    IInquiryRepository inquiryRepository,
    IBlobStorageService blobStorageService,
    IEmailService emailService,
    GeocoderTranslation geocoderTranslation) : FunderMapsController
{
    // GET: api/inquiry/stats
    /// <summary>
    ///     Return inquiry statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await inquiryRepository.CountAsync(TenantId),
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}
    /// <summary>
    ///     Return inquiry by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<Inquiry> GetAsync(int id)
        => await inquiryRepository.GetByIdAsync(id, TenantId);

    // GET: api/inquiry/building/{id}
    /// <summary>
    ///    Return all inquiries by building id.
    /// </summary>
    [HttpGet("building/{id}")]
    public async Task<IEnumerable<Inquiry>> GetAllByBuildingIdAsync(string id, [FromQuery] PaginationDto pagination)
    {
        var building = await geocoderTranslation.GetBuildingIdAsync(id);
        return await inquiryRepository.ListAllByBuildingIdAsync(pagination.Navigation, TenantId, building.Id).ToListAsync();
    }

    // GET: api/inquiry
    /// <summary>
    ///     Return all inquiries.
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Inquiry>> GetAllAsync([FromQuery] PaginationDto pagination)
        => await inquiryRepository.ListAllAsync(pagination.Navigation, TenantId).ToListAsync();

    // POST: api/inquiry
    /// <summary>
    ///     Create inquiry.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<Inquiry> CreateAsync([FromBody] Inquiry inquiry)
    {
        inquiry.Attribution.Creator = UserId;
        inquiry.Attribution.Owner = TenantId;

        if (inquiry.Attribution.Reviewer == UserId)
        {
            throw new AuthorizationException();
        }

        inquiry.Id = await inquiryRepository.AddAsync(inquiry);

        return await inquiryRepository.GetByIdAsync(inquiry.Id, TenantId);
    }

    // POST: api/inquiry/upload-document
    /// <summary>
    ///     Upload document to the backstore.
    /// </summary>
    [HttpPost("upload-document")]
    [RequestSizeLimit(128 * 1024 * 1024)]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UploadDocumentAsync([Required][FormFile(Core.Constants.AllowedFileMimes)] IFormFile input)
    {
        var storeFileName = FileHelper.GetUniqueName(input.FileName);
        await blobStorageService.StoreFileAsync(
            containerName: Core.Constants.InquiryStorageFolderName,
            fileName: storeFileName,
            contentType: input.ContentType,
            stream: input.OpenReadStream());

        var output = new DocumentDto()
        {
            Name = storeFileName,
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}/download
    /// <summary>
    ///     Retrieve document access link.
    /// </summary>
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> GetDocumentAccessLinkAsync(int id)
    {
        Inquiry inquiry = await inquiryRepository.GetByIdAsync(id, TenantId);
        Uri link = await blobStorageService.GetAccessLinkAsync(
            containerName: Core.Constants.InquiryStorageFolderName,
            fileName: inquiry.DocumentFile,
            hoursValid: 1);

        var result = new BlobAccessLinkDto()
        {
            AccessLink = link
        };

        return Ok(result);
    }

    // PUT: api/inquiry/{id}
    /// <summary>
    ///     Update inquiry by id.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] Inquiry inquiry)
    {
        inquiry.Id = id;
        inquiry.Attribution.Creator = UserId;
        inquiry.Attribution.Owner = TenantId;

        if (inquiry.Attribution.Reviewer == UserId)
        {
            throw new AuthorizationException();
        }

        await inquiryRepository.UpdateAsync(inquiry);

        // FUTURE: Does this make sense?
        // Only when this item was rejected can we move into
        // a pending state after update.
        if (inquiry.State.AuditStatus == AuditStatus.Rejected)
        {
            inquiry.State.TransitionToPending();
            await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);
        }

        return NoContent();
    }

    // POST: api/inquiry/{id}/reset
    /// <summary>
    ///     Reset inquiry status to pending by id.
    /// </summary>
    [HttpPost("{id:int}/reset")]
    [Authorize(Policy = "SuperuserAdministratorPolicy")]
    public async Task<IActionResult> ResetAsync(int id)
    {
        var inquiry = await inquiryRepository.GetByIdAsync(id, TenantId);

        inquiry.State.TransitionToPending();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

        return NoContent();
    }

    // POST: api/inquiry/{id}/status_review
    /// <summary>
    ///     Set inquiry status to review by id.
    /// </summary>
    [HttpPost("{id:int}/status_review")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> SetStatusReviewAsync(int id)
    {
        var inquiry = await inquiryRepository.GetByIdAsync(id, TenantId);
        var organization = await organizationRepository.GetByIdAsync(TenantId);
        var reviewer = await userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToReview();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = new[] { new EmailAddress(reviewer.Email, reviewer.ToString()) },
            Subject = "FunderMaps - Rapportage ter review",
            Template = "report-reviewer",
            Varaibles = new Dictionary<string, object>
            {
                { "id", inquiry.Id },
                { "creatorName", creator.ToString() },
                { "organizationName", organization.Name },
                { "reviewerName", reviewer.ToString() },
                { "documentName", inquiry.DocumentName },
            }
        });

        return NoContent();
    }

    // POST: api/inquiry/{id}/status_rejected
    /// <summary>
    ///     Set inquiry status to rejected by id.
    /// </summary>
    [HttpPost("{id:int}/status_rejected")]
    [Authorize(Policy = "VerifierAdministratorPolicy")]
    public async Task<IActionResult> SetStatusRejectedAsync(int id, StatusChangeDto input)
    {
        var inquiry = await inquiryRepository.GetByIdAsync(id, TenantId);
        var reviewer = await userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToRejected();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

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
                { "id", inquiry.Id },
                { "reviewerName", reviewer.ToString() },
                { "documentName", inquiry.DocumentName },
                { "motivation", input.Message },
            }
        });

        return NoContent();
    }

    // POST: api/inquiry/{id}/status_approved
    /// <summary>
    ///     Set inquiry status to done by id.
    /// </summary>
    [HttpPost("{id:int}/status_approved")]
    [Authorize(Policy = "VerifierAdministratorPolicy")]
    public async Task<IActionResult> SetStatusApprovedAsync(int id)
    {
        var inquiry = await inquiryRepository.GetByIdAsync(id, TenantId);
        var reviewer = await userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToDone();
        await inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry, TenantId);

        await emailService.SendAsync(new EmailMessage
        {
            ToAddresses = [
                new EmailAddress(creator.Email, creator.ToString()),
                new EmailAddress(reviewer.Email, reviewer.ToString())
            ],
            Subject = "FunderMaps - Rapportage is goedgekeurd",
            Template = "report-approved",
            Varaibles = new Dictionary<string, object>
            {
                { "id", inquiry.Id },
                { "reviewerName", reviewer.ToString() },
                { "documentName", inquiry.DocumentName },
            }
        });

        return NoContent();
    }

    // DELETE: api/inquiry/{id}
    /// <summary>
    ///     Delete inquiry by id.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Policy = "SuperuserAdministratorPolicy")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await inquiryRepository.DeleteAsync(id, TenantId);

        return NoContent();
    }
}
