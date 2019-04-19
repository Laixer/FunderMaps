using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps.Models.Identity
{
    public class FunderMapsUser : IdentityUser<Guid>
    {
        public FunderMapsUser()
        {
        }

        public FunderMapsUser(string email)
        {
            Email = email;
            UserName = email;
        }

        /// <summary>
        /// Gets or sets the given name for the user.
        /// </summary>
        [PersonalData]
        public virtual string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the last name for the user.
        /// </summary>
        [PersonalData]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Gets or sets a user avatar.
        /// </summary>
        [PersonalData]
        public virtual string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the job title for the user.
        /// </summary>
        [PersonalData]
        public virtual string JobTitle { get; set; }

        /// <summary>
        /// Reference to the attestation principal.
        /// </summary>
        [IgnoreDataMember]
        public virtual int? AttestationPrincipalId { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this
        /// user.
        /// </summary>
        [PersonalData]
        public new virtual bool? TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        [PersonalData]
        public new virtual bool? PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        [PersonalData]
        public new virtual bool? EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the lockout policy for this user.
        /// </summary>
        public new virtual bool? LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [IgnoreDataMember]
        public override string NormalizedUserName { get => base.NormalizedUserName; set => base.NormalizedUserName = value; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [IgnoreDataMember]
        public override string NormalizedEmail { get => base.NormalizedEmail; set => base.NormalizedEmail = value; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        [IgnoreDataMember]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        [IgnoreDataMember]
        public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        [IgnoreDataMember]
        public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        [IgnoreDataMember]
        public override int AccessFailedCount { get => base.AccessFailedCount; set => base.AccessFailedCount = value; }
    }
}
