using System;
using System.ComponentModel.DataAnnotations.Schema;
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
    }
}
