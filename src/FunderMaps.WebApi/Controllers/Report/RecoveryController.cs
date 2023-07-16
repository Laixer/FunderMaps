﻿using AutoMapper;
using FunderMaps.AspNetCore.DataAnnotations;
using FunderMaps.AspNetCore.DataTransferObjects;
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
[Route("api/recovery")]
public class RecoveryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly Core.AppContext _appContext;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRecoveryRepository _recoveryRepository;
    private readonly IBlobStorageService _blobStorageService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public RecoveryController(
        IMapper mapper,
        Core.AppContext appContext,
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IRecoveryRepository recoveryRepository,
        IBlobStorageService blobStorageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _recoveryRepository = recoveryRepository ?? throw new ArgumentNullException(nameof(recoveryRepository));
        _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
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
        => _recoveryRepository.GetByIdAsync(id);

    // GET: api/recovery
    /// <summary>
    ///     Return all recoveries.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<Recovery> GetAllAsync([FromQuery] PaginationDto pagination)
        => _recoveryRepository.ListAllAsync(pagination.Navigation);

    // POST: api/recovery
    /// <summary>
    ///     Create recovery.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public Task<Recovery> CreateAsync([FromBody] Recovery input)
    {
        // TODO: Creator must be self
        if (_appContext.UserId == input.Attribution.Reviewer)
        {
            throw new AuthorizationException();
        }

        return _recoveryRepository.AddGetAsync(input);
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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] Recovery input)
    {
        // var recovery = _mapper.Map<Recovery>(input);
        input.Id = id;

        if (input.Attribution.Creator == input.Attribution.Reviewer)
        {
            throw new AuthorizationException();
        }

        await _recoveryRepository.UpdateAsync(input);

        // FUTURE: Does this make sense?
        // Only when this item was rejected can we move into
        // a pending state after update.
        if (input.State.AuditStatus == AuditStatus.Rejected)
        {
            input.State.TransitionToPending();
            await _recoveryRepository.SetAuditStatusAsync(input.Id, input);
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
        var recovery = await _recoveryRepository.GetByIdAsync(id);

        recovery.State.TransitionToPending();
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery);

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
        // Act.
        var recovery = await _recoveryRepository.GetByIdAsync(id);
        // Organization organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        // User creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);
        // User reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer.Value);

        // Transition.
        recovery.State.TransitionToReview();

        // Act.
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery);

        // string subject = $"FunderMaps - Herstelrapportage ter review";

        // object header = new
        // {
        //     Title = subject,
        //     Preheader = "Herstelrapportage ter review wordt aangeboden."
        // };

        // string footer = "Dit bericht wordt verstuurd wanneer een herstelrapportage ter review wordt aangeboden.";

        // await _notifyService.NotifyAsync(new()
        // {
        //     Recipients = new List<string> { reviewer.Email },
        //     Subject = subject,
        //     Template = "RecoveryReview",
        //     Items = new Dictionary<string, object>
        //         {
        //             { "header", header },
        //             { "footer", footer },
        //             { "creator", creator.ToString() },
        //             { "organization", organization.ToString() },
        //             { "recovery", recovery },
        //             { "redirect_link", $"{Request.Scheme}://{Request.Host}/recovery/{recovery.Id}" },
        //         },
        // });

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
        // Act.
        var recovery = await _recoveryRepository.GetByIdAsync(id);
        // Organization organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        // User reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer.Value);
        // User creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);

        // Transition.
        recovery.State.TransitionToRejected();

        // Act.
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery);

        // string subject = $"FunderMaps - Herstelrapportage afgekeurd";

        // object header = new
        // {
        //     Title = subject,
        //     Preheader = "Herstelrapportage is afgekeurd."
        // };

        // string footer = "Dit bericht wordt verstuurd wanneer een herstelrapportage is afgekeurd.";

        // await _notifyService.NotifyAsync(new()
        // {
        //     Recipients = new List<string> { creator.Email },
        //     Subject = subject,
        //     Template = "RecoveryRejected",
        //     Items = new Dictionary<string, object>
        //         {
        //             { "header", header },
        //             { "footer", footer },
        //             { "reviewer", reviewer.ToString() },
        //             { "organization", organization.ToString() },
        //             { "recovery", recovery },
        //             { "message", input.Message },
        //             { "redirect_link", $"{Request.Scheme}://{Request.Host}/recovery/{recovery.Id}" },
        //         },
        // });

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
        // Act.
        var recovery = await _recoveryRepository.GetByIdAsync(id);
        // Organization organization = await _organizationRepository.GetByIdAsync(_appContext.TenantId);
        // User reviewer = await _userRepository.GetByIdAsync(recovery.Attribution.Reviewer.Value);
        // User creator = await _userRepository.GetByIdAsync(recovery.Attribution.Creator);

        // Transition.
        recovery.State.TransitionToDone();

        // Act.
        await _recoveryRepository.SetAuditStatusAsync(recovery.Id, recovery);

        // string subject = $"FunderMaps - Herstelrapportage goedgekeurd";

        // object header = new
        // {
        //     Title = subject,
        //     Preheader = "Herstelrapportage is goedgekeurd."
        // };

        // string footer = "Dit bericht wordt verstuurd wanneer een herstelrapportage is goedgekeurd.";

        // await _notifyService.NotifyAsync(new()
        // {
        //     Recipients = new List<string> { creator.Email },
        //     Subject = "FunderMaps - Herstelrapportage goedgekeurd",
        //     Template = "RecoveryApproved",
        //     Items = new Dictionary<string, object>
        //         {
        //             { "header", header },
        //             { "footer", footer },
        //             { "reviewer", reviewer.ToString() },
        //             { "organization", organization.ToString() },
        //             { "recovery", recovery },
        //         },
        // });

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
        await _recoveryRepository.DeleteAsync(id);

        return NoContent();
    }
}
