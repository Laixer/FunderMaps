﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Identity
{
    /// <summary>
    /// Application user.
    /// </summary>
    public class FunderMapsUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public FunderMapsUser() { }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="email">User email.</param>
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
