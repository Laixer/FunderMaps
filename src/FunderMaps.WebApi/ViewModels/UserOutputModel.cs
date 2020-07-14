using System;

namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// User output mode.
    /// </summary>
    public sealed class UserOutputModel : PrincipalOutputModel
    {
        /// <summary>
        /// Indicates if 2FA configured on the account.
        /// </summary>
        public bool? TwoFactorEnabled { get; set; }

        /// <summary>
        /// Indicates if email is confirmed on the account.
        /// </summary>
        public bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Indicates if account is locked-out.
        /// </summary>
        public bool? LockoutEnabled { get; set; }

        /// <summary>
        /// Indicates if phone number is confirmed on the account.
        /// </summary>
        public bool? PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Number of failed login attempts.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Locked-out till date, null if not locked-out
        /// </summary>
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
