using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Email
{
    /// <summary>
    ///     Email address.
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        ///     Name corresponding to address.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Email address.
        /// </summary>
        [Required, EmailAddress]
        public string Address { get; set; }

        public override string ToString() => Address;
    }
}
