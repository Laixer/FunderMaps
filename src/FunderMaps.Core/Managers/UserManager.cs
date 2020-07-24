using FunderMaps.Core.Entities;
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
            return await _userRepository.GetByIdAsync(id).ConfigureAwait(false);
        }

        public virtual async ValueTask<User> GetByEmailAsync(string email)
        {
            Validator.ValidateValue(email, new ValidationContext(email), new List<EmailAddressAttribute> { new EmailAddressAttribute() });

            return await _userRepository.GetByEmailAsync(email).ConfigureAwait(false);
        }

        public virtual async ValueTask<User> CreateAsync(User user, string plainPassword = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.InitializeDefaults();

            Validator.ValidateObject(user, new ValidationContext(user), true);

            var id = await _userRepository.AddAsync(user).ConfigureAwait(false);
            user.InitializeDefaults(await _userRepository.GetByIdAsync(id).ConfigureAwait(false));

            if (!string.IsNullOrEmpty(plainPassword))
            {
                // FUTURE: Want to do this in a single call.
                var passwordHash = _passwordHasher.HashPassword(plainPassword);
                await _userRepository.SetPasswordHashAsync(user, passwordHash).ConfigureAwait(false);
            }

            return user;
        }

        public virtual IAsyncEnumerable<User> GetAllAsync(INavigation navigation)
        {
            return _userRepository.ListAllAsync(navigation);
        }

        public virtual async ValueTask UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.InitializeDefaults(await _userRepository.GetByIdAsync(user.Id).ConfigureAwait(false));

            Validator.ValidateObject(user, new ValidationContext(user), true);

            await _userRepository.UpdateAsync(user).ConfigureAwait(false);
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

            user.InitializeDefaults(await _userRepository.GetByIdAsync(user.Id).ConfigureAwait(false));

            var currentPasswordHash = await _userRepository.GetPasswordHashAsync(user).ConfigureAwait(false);
            if (!_passwordHasher.IsPasswordValid(currentPasswordHash, currentPassword))
            {
                throw new Exception(); // TODO: Auth invalid ex.
            }

            var newPasswordHash = _passwordHasher.HashPassword(newPassword);
            await _userRepository.SetPasswordHashAsync(user, newPasswordHash).ConfigureAwait(false);
        }

        public virtual async ValueTask DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id).ConfigureAwait(false);
        }
    }
}
