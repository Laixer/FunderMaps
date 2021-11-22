using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for organization setup.
/// </summary>
/// <remarks>
///     Organization setup converts an already existing organization
///     proposal into a full organization.
/// </remarks>
[AllowAnonymous]
public class OrganizationSetupController : ControllerBase
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationSetupController(IOrganizationRepository organizationRepository, IPasswordHasher passwordHasher)
    {
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    // POST: api/organization/{id}/setup
    /// <summary>
    ///     Create organization from organization proposal.
    /// </summary>
    /// <remarks>
    ///     This is an unauthorized call, therefore this
    ///     call deliberately returns nothing.
    /// </remarks>
    [HttpPost("organization/{id:guid}/setup")]
    public async Task<IActionResult> CreateAsync(Guid id, [FromBody] OrganizationSetupDto input)
    {
        // Map.
        var user = new User { Email = input.Email };

        // Act.
        var passwordHash = _passwordHasher.HashPassword(input.Password);
        await _organizationRepository.AddFromProposalAsync(id, user.Email, passwordHash);

        // Return.
        return NoContent();
    }
}
