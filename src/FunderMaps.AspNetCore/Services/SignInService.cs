using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FunderMaps.AspNetCore.Services;

/// <summary>
///     Provides the APIs for authentication.
/// </summary>
public class SignInService
{
    /// <summary>
    ///     The <see cref="IUserRepository"/> used.
    /// </summary>
    public IUserRepository UserRepository { get; }

    /// <summary>
    ///     The <see cref="IOrganizationUserRepository"/> used.
    /// </summary>
    public IOrganizationUserRepository OrganizationUserRepository { get; }

    /// <summary>
    ///     The <see cref="IOrganizationRepository"/> used.
    /// </summary>
    public IOrganizationRepository OrganizationRepository { get; }

    /// <summary>
    ///     The <see cref="IPasswordHasher"/> used.
    /// </summary>
    public IPasswordHasher PasswordHasher { get; }

    /// <summary>
    ///     Gets the <see cref="ILogger"/> used to log messages.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    ///     Creates a new instance.
    /// </summary>
    public SignInService(
        IUserRepository userRepository,
        IOrganizationUserRepository organizationUserRepository,
        IOrganizationRepository organizationRepository,
        IPasswordHasher passwordHasher,
        ILogger<SignInService> logger)
    {
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        OrganizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
        OrganizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        PasswordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Test if the provided password is valid for the user.
    /// </summary>
    /// <param name="id">The user id whose password should be checked.</param>
    /// <param name="password">The password to be checked against the user.</param>
    /// <returns><c>True</c> if the specified password is valid for the user, otherwise false.</returns>
    public virtual async Task<bool> CheckPasswordAsync(Guid id, string password)
    {
        var passwordHash = await UserRepository.GetPasswordHashAsync(id);
        return PasswordHasher.IsPasswordValid(passwordHash, password);
    }

    /// <summary>
    ///     Set the password for the user.
    /// </summary>
    /// <param name="id">The user id whose password should be set.</param>
    /// <param name="password">The plaintext password to be set on the user.</param>
    public virtual async Task SetPasswordAsync(Guid id, string password)
    {
        var passwordHash = PasswordHasher.HashPassword(password);
        await UserRepository.SetPasswordHashAsync(id, passwordHash);
    }

    private async Task<ClaimsIdentity> CreateClaimsIdentityAsync(User user, string authenticationType)
    {
        if (await UserRepository.GetAccessFailedCount(user.Id) > 10)
        {
            Logger.LogWarning($"User '{user}' locked out.");

            throw new AuthenticationException();
        }

        await UserRepository.ResetAccessFailed(user.Id);
        await UserRepository.RegisterAccess(user.Id);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };

        var organizationId = await OrganizationUserRepository.GetOrganizationByUserIdAsync(user.Id);
        var organization = await OrganizationRepository.GetByIdAsync(organizationId);
        var organizationRole = await OrganizationUserRepository.GetOrganizationRoleByUserIdAsync(user.Id);

        claims.Add(new Claim(FunderMapsAuthenticationClaimTypes.Tenant, organization.Id.ToString()));
        claims.Add(new Claim(FunderMapsAuthenticationClaimTypes.TenantRole, organizationRole.ToString()));

        Logger.LogDebug($"User '{user}' signin was successful.");

        return new(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="principal"/>.
    /// </summary>
    /// <param name="principal">The principal to sign in.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> UserIdSignInAsync(Guid id, string authenticationType)
    {
        var user = await UserRepository.GetByIdAsync(id);

        var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

        return new ClaimsPrincipal(claimsIdentity);
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="principal"/>.
    /// </summary>
    /// <param name="principal">The principal to sign in.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> AuthKeySignInAsync(string key, string authenticationType)
    {
        // if (key == "")
        // {
        //     var user = await UserRepository.GetByIdAsync(Guid.Parse("7a015c0a-55ce-4b8e-84b5-784bd3363d5b"));

        //     var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

        //     return new ClaimsPrincipal(claimsIdentity);
        // }

        throw new AuthenticationException();
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="email"/> and <paramref name="password"/> combination.
    /// </summary>
    /// <param name="email">The user email to sign in.</param>
    /// <param name="password">The password to attempt to authenticate.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> PasswordSignInAsync(string email, string password, string authenticationType)
    {
        if (await UserRepository.GetByEmailAsync(email) is not User user)
        {
            throw new AuthenticationException();
        }

        if (!await CheckPasswordAsync(user.Id, password))
        {
            Logger.LogWarning($"User '{user}' failed to provide the correct password.");

            await UserRepository.BumpAccessFailed(user.Id);

            throw new AuthenticationException();
        }

        Logger.LogInformation($"User '{user}' password signin was successful.");

        var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}
