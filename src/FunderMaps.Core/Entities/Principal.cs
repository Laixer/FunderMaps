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
        public int Id { get; set; }

        [MaxLength(256)]
        public string NickName { get; set; }

        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string MiddleName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [MaxLength(256)]
        public string Email { get; set; }

        [IgnoreDataMember]
        public int? _Organization { get; set; }

        [MaxLength(16)]
        public string Phone { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
