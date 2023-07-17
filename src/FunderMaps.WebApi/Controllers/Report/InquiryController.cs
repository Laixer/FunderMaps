using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
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
///     Endpoint controller for inquiry operations.
/// </summary>
[Route("api/inquiry")]
public class InquiryController : ControllerBase
{
    private readonly Core.AppContext _appContext;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IInquiryRepository _inquiryRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IEmailService _emailService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public InquiryController(
        Core.AppContext appContext,
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IInquiryRepository inquiryRepository,
        IBlobStorageService blobStorageService,
        IEmailService emailService)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _inquiryRepository = inquiryRepository ?? throw new ArgumentNullException(nameof(inquiryRepository));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    // GET: api/inquiry/stats
    /// <summary>
    ///     Return inquiry statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await _inquiryRepository.CountAsync(),
        };

        return Ok(output);
    }

    // GET: api/inquiry/{id}
    /// <summary>
    ///     Return inquiry by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public Task<Inquiry> GetAsync(int id)
        => _inquiryRepository.GetByIdAsync(id);

    // GET: api/inquiry
    /// <summary>
    ///     Return all inquiries.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<Inquiry> GetAllAsync([FromQuery] PaginationDto pagination)
        => _inquiryRepository.ListAllAsync(pagination.Navigation);

    // POST: api/inquiry
    /// <summary>
    ///     Create inquiry.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public Task<Inquiry> CreateAsync([FromBody] Inquiry inquiry)
    {
        if (_appContext.UserId == inquiry.Attribution.Reviewer)
        {
            throw new AuthorizationException();
        }

        return _inquiryRepository.AddGetAsync(inquiry);
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
        await _blobStorageService.StoreFileAsync(
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

    // GET: api/inquiry/download
    /// <summary>
    ///     Retrieve document access link.
    /// </summary>
    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> GetDocumentAccessLinkAsync(int id)
    {
        Inquiry inquiry = await _inquiryRepository.GetByIdAsync(id);
        Uri link = await _blobStorageService.GetAccessLinkAsync(
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

        if (_appContext.UserId == inquiry.Attribution.Reviewer)
        {
            throw new AuthorizationException();
        }

        await _inquiryRepository.UpdateAsync(inquiry);

        // FUTURE: Does this make sense?
        // Only when this item was rejected can we move into
        // a pending state after update.
        if (inquiry.State.AuditStatus == AuditStatus.Rejected)
        {
            inquiry.State.TransitionToPending();
            await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);
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
        var inquiry = await _inquiryRepository.GetByIdAsync(id);

        inquiry.State.TransitionToPending();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

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
        var inquiry = await _inquiryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        var reviewer = await _userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToReview();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

        await _emailService.SendAsync(new EmailMessage
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
        var inquiry = await _inquiryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        var reviewer = await _userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToRejected();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

        await _emailService.SendAsync(new EmailMessage
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
        var inquiry = await _inquiryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        var reviewer = await _userRepository.GetByIdAsync(inquiry.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(inquiry.Attribution.Creator);

        inquiry.State.TransitionToDone();
        await _inquiryRepository.SetAuditStatusAsync(inquiry.Id, inquiry);

        await _emailService.SendAsync(new EmailMessage
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
        await _inquiryRepository.DeleteAsync(id);

        return NoContent();
    }
}
