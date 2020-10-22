using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     User signin result DTO.
    /// </summary>
    public class SignInSecurityTokenDto
    {
        /// <summary>
        ///     Authentication token identifier.
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        ///     Authentication issuer.
        /// </summary>
        [Required]
        public string Issuer { get; set; }

        /// <summary>
        ///     Authentication token.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        ///     Authentication token valid from datetime.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        ///     Authentication token valid until datetime.
        /// </summary>
        public DateTime ValidTo { get; set; }
    }
}
