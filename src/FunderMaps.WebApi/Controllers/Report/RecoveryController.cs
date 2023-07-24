using FunderMaps.AspNetCore.Authentication;
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
using System.Security.Claims;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for recovery operations.
/// </summary>
[Route("api/recovery")]
public class RecoveryController : ControllerBase
{
    private readonly Core.AppContext _appContext;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRecoveryRepository _recoveryRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IEmailService _emailService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public RecoveryController(
        Core.AppContext appContext,
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IRecoveryRepository recoveryRepository,
        IBlobStorageService blobStorageService,
        IEmailService emailService)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _recoveryRepository = recoveryRepository ?? throw new ArgumentNullException(nameof(recoveryRepository));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    // GET: api/recovery/stats
    /// <summary>
    ///     Return recovery statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await _recoveryRepository.CountAsync(),
        };

        return Ok(output);
    }

    // GET: api/recovery/{id}
    /// <summary>
    ///     Return recovery by id.
    /// </summary>
    [HttpGet("{id:int}")]
    public Task<Recovery> GetAsync(int id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recovery = _recoveryRepository.GetByIdAsync(id, tenantId);

        return recovery;
    }

    // GET: api/recovery
    /// <summary>
    ///     Return all recoveries.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<Recovery> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await foreach (var recovery in _recoveryRepository.ListAllAsync(pagination.Navigation, tenantId))
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
    public Task<Recovery> CreateAsync([FromBody] Recovery recovery)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        recovery.Attribution.Creator = userId;
        recovery.Attribution.Owner = tenantId;

        if (recovery.Attribution.Reviewer == userId)
        {
            throw new AuthorizationException();
        }

        return _recoveryRepository.AddGetAsync(recovery);
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
        await _blobStorageService.StoreFileAsync(
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
        var recovery = await _recoveryRepository.GetByIdAsync(id);
        Uri link = await _blobStorageService.GetAccessLinkAsync(
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
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        recovery.Id = id;
        recovery.Attribution.Creator = userId;
        recovery.Attribution.Owner = tenantId;

        if (recovery.Attribution.Reviewer == userId)
        {
            throw new AuthorizationException();
        }

        await _recoveryRepository.UpdateAsync(recovery);

        // FUTURE: Does this make sense?
        // Only when this item was rejected can we move into
        // a pending state after update.
        if (recovery.State.AuditStatus == AuditStatus.Rejected)
        {
            recovery.State.TransitionToPending();
            await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);
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
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recovery = await _recoveryRepository.GetByIdAsync(id);

        recovery.State.TransitionToPending();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

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
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recovery = await _recoveryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(tenantId);
        var reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToReview();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

        await _emailService.SendAsync(new EmailMessage
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
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recovery = await _recoveryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(tenantId);
        var reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToRejected();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

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
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var recovery = await _recoveryRepository.GetByIdAsync(id);
        var organization = await _organizationRepository.GetByIdAsync(tenantId);
        var reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer);
        var creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);

        recovery.State.TransitionToDone();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery, tenantId);

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
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await _recoveryRepository.DeleteAsync(id, tenantId);

        return NoContent();
    }
}
