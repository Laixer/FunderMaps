using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FunderMaps.Core.Managers
{
    /// <summary>
    ///     User manager.
    /// </summary>
    public class UserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserManager(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public virtual async ValueTask<User> GetAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _userRepository.GetByIdAsync(id);
        }

        public virtual async ValueTask<User> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _userRepository.GetByEmailAsync(email);
        }

        public virtual async ValueTask<User> CreateAsync(User user, string plainPassword = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.InitializeDefaults();
            user.Validate();

            var id = await _userRepository.AddAsync(user);
            user.InitializeDefaults(await _userRepository.GetByIdAsync(id));

            if (!string.IsNullOrEmpty(plainPassword))
            {
                // FUTURE: Want to do this in a single call.
                var passwordHash = _passwordHasher.HashPassword(plainPassword);
                await _userRepository.SetPasswordHashAsync(user, passwordHash);
            }

            return user;
        }

        public virtual IAsyncEnumerable<User> GetAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            return _userRepository.ListAllAsync(navigation);
        }

        public virtual async ValueTask UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.InitializeDefaults(await _userRepository.GetByIdAsync(user.Id));
            user.Validate();

            await _userRepository.UpdateAsync(user);
        }

        public virtual async ValueTask ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentNullException(nameof(currentPassword));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            user.InitializeDefaults(await _userRepository.GetByIdAsync(user.Id));
            user.Validate();

            if (!await CheckPasswordAsync(user, currentPassword))
            {
                throw new InvalidCredentialException();
            }

            var newPasswordHash = _passwordHasher.HashPassword(newPassword);
            await _userRepository.SetPasswordHashAsync(user, newPasswordHash);
        }

        public virtual async ValueTask<bool> CheckPasswordAsync(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var currentPasswordHash = await _userRepository.GetPasswordHashAsync(user);
            return _passwordHasher.IsPasswordValid(currentPasswordHash, password);
        }

        public virtual async ValueTask DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _userRepository.DeleteAsync(id);
        }

        public virtual async ValueTask IncreaseAccessFailedCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _userRepository.BumpAccessFailed(user);
        }

        public virtual async ValueTask ResetAccessFailedCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _userRepository.ResetAccessFailed(user);
        }

        public virtual async ValueTask IncreaseAccessCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _userRepository.RegisterAccess(user);
        }

        public virtual async ValueTask<bool> IsLockedOutAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await _userRepository.IsLockedOutAsync(user);
        }
    }
}
