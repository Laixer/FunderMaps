using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    // TODO: can be removed.
    /// <summary>
    /// Principal entity.
    /// </summary>
    public class Principal : BaseEntity
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User nickname.
        /// </summary>
        [MaxLength(256)]
        public string NickName { get; set; }

        /// <summary>
        /// User firstname.
        /// </summary>
        [MaxLength(256)]
        public string FirstName { get; set; }

        /// <summary>
        /// User middlename.
        /// </summary>
        [MaxLength(256)]
        public string MiddleName { get; set; }

        /// <summary>
        /// User lastname.
        /// </summary>
        [MaxLength(256)]
        public string LastName { get; set; }

        /// <summary>
        /// Unique email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [MaxLength(256)]
        public string Email { get; set; }

        /// <summary>
        /// Organization identifier.
        /// </summary>
        [IgnoreDataMember]
        public int? _Organization { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        [MaxLength(16)]
        public string Phone { get; set; }

        /// <summary>
        /// Organization object.
        /// </summary>
        public Organization Organization { get; set; }
    }
}
