using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FunderMaps.ViewModels
{
    /// <summary>
    /// Security principal.
    /// </summary>
    public class PrincipalOutputModel
    {
        /// <summary>
        /// Principal id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Principal email.
        /// </summary>
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Application roles.
        /// </summary>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Security claims.
        /// </summary>
        public IList<Claim> Claims { get; set; }
    }
}
