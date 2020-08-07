using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     User signin result DTO.
    /// </summary>
    public class SignInSecurityTokenDto
    {
        /// <summary>
        ///     Authentication token.
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        ///     Authentication token validity.
        /// </summary>
        [Required]
        public int TokenValidity { get; set; }
    }
}
