using FunderMaps.Core.Authentication;
using FunderMaps.Core.Email;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FunderMaps.Core.Services;

/// <summary>
///     Provides the APIs for authentication.
/// </summary>
public class SignInService(
    IUserRepository userRepository,
    IOrganizationUserRepository organizationUserRepository,
    PasswordHasher passwordHasher,
    IEmailService emailService,
    ILogger<SignInService> logger)
{
    /// <summary>
    ///   The number of failed access attempts allowed before a user is locked out.
    /// </summary>
    private const int MaxFailedAccessAttempts = 10;

    /// <summary>
    ///     Send a password reset email to the user.
    /// </summary>
    /// <remarks>
    ///     This method will not throw an exception if the user does not exist.
    /// </remarks>
    public virtual async Task ResetPasswordAsync(string email)
    {
        try
        {
            var user = await userRepository.GetByEmailAsync(email.Trim());

            // TOOD: Generate random code and send with email.
            await emailService.SendAsync(new EmailMessage
            {
                ToAddresses = new[] { new EmailAddress(user.Email, user.ToString()) },
                Subject = "FunderMaps - Wachtwoord reset",
                Template = "reset-password",
                Varaibles = new Dictionary<string, object>
                {
                    { "creatorName", user.ToString() },
                    { "resetToken", "123456" },
                }
            });
        }
        catch
        {
            logger.LogWarning("User '{email}' requested password reset, but does not exist.", email);
        }
    }

    /// <summary>
    ///     Test if the provided password is valid for the user.
    /// </summary>
    /// <param name="id">The user id whose password should be checked.</param>
    /// <param name="password">The password to be checked against the user.</param>
    /// <returns><c>True</c> if the specified password is valid for the user, otherwise false.</returns>
    public virtual async Task<bool> CheckPasswordAsync(Guid id, string password)
    {
        var passwordHash = await userRepository.GetPasswordHashAsync(id);
        return passwordHash is not null && passwordHasher.IsPasswordValid(passwordHash, password.Trim());
    }

    /// <summary>
    ///     Set the password for the user.
    /// </summary>
    /// <param name="id">The user id whose password should be set.</param>
    /// <param name="password">The plaintext password to be set on the user.</param>
    public virtual async Task SetPasswordAsync(Guid id, string password)
    {
        var passwordHash = passwordHasher.HashPassword(password.Trim());
        await userRepository.SetPasswordHashAsync(id, passwordHash);
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user to sign in.</param>
    /// <param name="authenticationType">The authentication type.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    private async Task<ClaimsIdentity> CreateClaimsIdentityAsync(User user, string authenticationType)
    {
        if (await userRepository.GetAccessFailedCount(user.Id) > MaxFailedAccessAttempts)
        {
            logger.LogWarning("User '{user}' locked out.", user);

            throw new AuthenticationException();
        }

        await userRepository.ResetAccessFailed(user.Id);
        await userRepository.RegisterAccess(user.Id);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var organizationIds = await organizationUserRepository.ListAllOrganizationIdByUserIdAsync(user.Id).ToListAsync();
        foreach (var organizationId in organizationIds)
        {
            claims.Add(new Claim(FunderMapsAuthenticationClaimTypes.Tenant, organizationId.ToString()));
        }

        // FUTURE: There is a role per organization, but we only support one role for now.
        var organizationRole = await organizationUserRepository.GetOrganizationRoleByUserIdAsync(user.Id, organizationIds.First());
        if (organizationRole is not null)
        {
            claims.Add(new Claim(FunderMapsAuthenticationClaimTypes.TenantRole, organizationRole.ToString() ?? string.Empty));
        }

        logger.LogDebug("User '{user}' signin was successful.", user);

        return new(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="principal"/>.
    /// </summary>
    /// <param name="principal">The principal to sign in.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> UserIdSignInAsync(Guid id, string authenticationType)
    {
        try
        {
            if (await userRepository.GetByIdAsync(id) is not User user)
            {
                throw new AuthenticationException();
            }

            var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

            return new ClaimsPrincipal(claimsIdentity);
        }
        catch (EntityNotFoundException)
        {
            throw new AuthenticationException();
        }
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="principal"/>.
    /// </summary>
    /// <param name="principal">The principal to sign in.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> AuthKeySignInAsync(string key, string authenticationType)
    {
        try
        {
            if (await userRepository.GetByAuthKeyAsync(key.Trim()) is not User user)
            {
                throw new AuthenticationException();
            }

            var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

            return new ClaimsPrincipal(claimsIdentity);
        }
        catch (EntityNotFoundException)
        {
            throw new AuthenticationException();
        }
    }

    /// <summary>
    ///     Attempts to sign in the specified <paramref name="email"/> and <paramref name="password"/> combination.
    /// </summary>
    /// <param name="email">The user email to sign in.</param>
    /// <param name="password">The password to attempt to authenticate.</param>
    /// <returns>Instance of <see cref="TokenContext"/>.</returns>
    public virtual async Task<ClaimsPrincipal> PasswordSignInAsync(string email, string password, string authenticationType)
    {
        try
        {
            if (await userRepository.GetByEmailAsync(email.Trim()) is not User user)
            {
                throw new AuthenticationException();
            }

            if (!await CheckPasswordAsync(user.Id, password.Trim()))
            {
                logger.LogWarning("User '{user}' failed to provide the correct password.", user);

                await userRepository.BumpAccessFailed(user.Id);

                throw new AuthenticationException();
            }

            logger.LogInformation("User '{user}' password signin was successful.", user);

            var claimsIdentity = await CreateClaimsIdentityAsync(user, authenticationType);

            return new ClaimsPrincipal(claimsIdentity);
        }
        catch (EntityNotFoundException)
        {
            throw new AuthenticationException();
        }
    }
}
