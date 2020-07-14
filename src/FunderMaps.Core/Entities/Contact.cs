using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Contact information.
    /// </summary>
    public class Contact : BaseEntity
    {
        /// <summary>
        /// Contact email.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Contact name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contact phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
